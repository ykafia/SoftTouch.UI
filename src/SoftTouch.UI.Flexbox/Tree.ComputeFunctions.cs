using System.Security.Cryptography;

namespace SoftTouch.UI.Flexbox;


public partial class FlexTree
{
    private void PrecomputeParentSizes(FlexQueue secondPass, FlexQueue forwardQueue)
    {
        while (secondPass.Count > 0)
        {
            var e = secondPass.Dequeue() ?? throw new NullReferenceException();
            forwardQueue.Enqueue(e);

            if (e.Value.Width is null)
            {
                var childrenCount = 0;
                foreach (var child in Adjacency[e])
                {
                    if (child.Value is BoxElement cbv)
                    {
                        if (cbv.Width is not null && cbv.Width?.Kind == ViewNumberKind.Number)
                        {

                            if (cbv.FlexDirection == FlexDirection.Row && cbv.Position == FlexPosition.Relative)
                                e.Value.Width = (e.Value.Width ?? 0) + (cbv.Width ?? 0) + (cbv.MarginLeft ?? 0) + (cbv.MarginRight ?? 0);

                            if (cbv.FlexDirection == FlexDirection.Column && cbv.Position == FlexPosition.Relative)
                                e.Value.Width =
                                    (e.Value.Width ?? 0)
                                    + ViewNumber.MaxMagnitude(
                                        e.Value.Width ?? 0,
                                        (cbv.Width ?? 0) + (cbv.MarginLeft ?? 0) + (cbv.MarginRight ?? 0)
                                    );
                        }
                        if (cbv.Position == FlexPosition.Relative)
                            childrenCount += 1;
                    }
                    else if (child.Value is TextStyle ctv)
                        if (ctv.Width is not null && ctv.Width?.Kind == ViewNumberKind.Number)
                            e.Value.Width = (e.Value.Width ?? 0) + ctv.Width;
                }

                if (e.Value is BoxElement ebv)
                    ebv.Width +=
                        (ebv.PaddingLeft ?? 0)
                        + (ebv.PaddingRight ?? 0)
                        + (ebv.FlexDirection == FlexDirection.Row ? (childrenCount - 1) * (ebv.Gap ?? 0) : 0);
            }

            if (e.Value.Height is null)
            {
                var childrenCount = 0;

                foreach (var child in Adjacency[e])
                {
                    if (child.Value is BoxElement cbv && cbv.Width?.Kind == ViewNumberKind.Number)
                    {
                        if (cbv.Width is not null)
                        {
                            if (cbv.FlexDirection == FlexDirection.Column && cbv.Position == FlexPosition.Relative)
                                e.Value.Height = (e.Value.Height ?? 0) + (cbv.Height ?? 0) + (cbv.Top ?? 0) + (cbv.Bottom ?? 0);

                            if (cbv.FlexDirection == FlexDirection.Row && cbv.Position == FlexPosition.Relative)
                                e.Value.Height =
                                    (e.Value.Height ?? 0)
                                    + ViewNumber.MaxMagnitude(
                                        e.Value.Height ?? 0,
                                        (cbv.Height ?? 0) + (cbv.Top ?? 0) + (cbv.Bottom ?? 0)
                                    );
                        }
                        if (cbv.Position == FlexPosition.Relative)
                            childrenCount += 1;
                    }
                    else if (child.Value is TextStyle ctv)
                        if (ctv.Height is not null && ctv.Height?.Kind == ViewNumberKind.Number)
                            e.Value.Height = (e.Value.Height ?? 0) + ctv.Height;
                }

                if (e.Value is BoxElement ebv)
                    ebv.Height +=
                        (ebv.PaddingTop ?? 0)
                        + (ebv.PaddingBottom ?? 0)
                        + (ebv.FlexDirection == FlexDirection.Column ? (childrenCount - 1) * (ebv.Gap ?? 0) : 0);
            }
        }
    }



    /// <summary>
    /// Computes the width of each element based on existing width and left/right/top/bottom
    /// </summary>
    /// <param name="element"></param>
    /// <param name="parent"></param>
    void ResolveFlexProperties(FlexNode element, FlexNode parent)
    {
        if (element.Value is BoxElement box)
        {
            // If absolute then the position is computed from the parent's position


            box.X = (box.Position, box.Width, box.Left, box.Right, parent.Value) switch
            {
                (FlexPosition.Relative, null, ViewNumber l, _, _) => box.X + l,
                (FlexPosition.Relative, null, null, ViewNumber r, _) => box.X - r,
                (FlexPosition.Absolute, null, ViewNumber l, _, BoxElement p) => p.X + l,
                (FlexPosition.Absolute, null, null, ViewNumber r, BoxElement p) => p.X + p.Width - r,
                (FlexPosition.Absolute, null, null, null, BoxElement p) => p.X,
                _ => box.X
            };
            box.Width = (box.Position, box.Width, box.Left, box.Right, parent.Value) switch
            {
                (FlexPosition.Absolute, null, ViewNumber l, ViewNumber r, BoxElement p) => p.Width - l - r,
                (FlexPosition.Absolute, null, ViewNumber l, null, BoxElement p) => p.Width - l,
                (FlexPosition.Absolute, null, null, ViewNumber r, BoxElement p) => p.Width - r,
                (FlexPosition.Absolute, null, null, null, _) => 0,
                _ => box.Width,
            };
            box.Y = (box.Position, box.Height, box.Top, box.Bottom, parent.Value) switch
            {
                (FlexPosition.Relative, null, ViewNumber t, _, _) => box.Y + t,
                (FlexPosition.Relative, null, null, ViewNumber b, _) => box.Y - b,
                (FlexPosition.Absolute, null, ViewNumber t, _, BoxElement p) => p.X + t,
                (FlexPosition.Absolute, null, null, ViewNumber b, BoxElement p) => p.Y + p.Width - b,
                (FlexPosition.Absolute, null, null, null, BoxElement p) => p.Y,
                _ => box.Y
            };
            box.Height = (box.Position, box.Width, box.Left, box.Right, parent.Value) switch
            {
                (FlexPosition.Absolute, null, ViewNumber t, ViewNumber b, BoxElement p) => p.Width - t - b,
                (FlexPosition.Absolute, null, ViewNumber t, null, BoxElement p) => p.Height - t,
                (FlexPosition.Absolute, null, null, ViewNumber b, BoxElement p) => p.Height - b,
                (FlexPosition.Absolute, null, null, null, _) => 0,
                _ => box.Height,
            };

            box.X ??= 0;
            box.Y ??= 0;
        }
    }

    void ResolveAlignSelf(FlexNode element, FlexNode parent)
    {
        // Resolving Align self
        if (element.Value is BoxElement e && parent.Value is BoxElement p)
        {
            e.Y = (e.Position, p.FlexDirection, e.AlignSelf) switch
            {
                (FlexPosition.Relative, FlexDirection.Row, FlexAlignment.Center) => (e.Y ?? 0) + (p.Height ?? 0) / 2 - (e.Height ?? 0 / 2),
                (FlexPosition.Relative, FlexDirection.Row, FlexAlignment.FlexEnd) => (e.Y ?? 0) + (p.Height ?? 0) - (e.Height ?? 0 / 2) - (p.PaddingBottom ?? 0) - (p.PaddingTop ?? 0),
                (FlexPosition.Absolute, FlexDirection.Row, FlexAlignment.Center) => (e.Y ?? 0) + (p.Height ?? 0) / 2 - (e.Height ?? 0 / 2),
                (FlexPosition.Absolute, FlexDirection.Row, FlexAlignment.FlexEnd) => (e.Y ?? 0) + (p.Height ?? 0) - (e.Height ?? 0 / 2) - (p.PaddingBottom ?? 0) - (p.PaddingTop ?? 0),
                _ => e.Y
            };
            if (p.FlexDirection == FlexDirection.Row && e.AlignSelf == FlexAlignment.Stretch)
                e.Height = (p.Height ?? 0) + (p.PaddingBottom ?? 0) + (p.PaddingTop ?? 0);
            e.X = (e.Position, p.FlexDirection, e.AlignSelf) switch
            {
                (FlexPosition.Relative, FlexDirection.Column, FlexAlignment.Center) => (e.X ?? 0) + (p.Width ?? 0) / 2 - (e.Width ?? 0 / 2),
                (FlexPosition.Relative, FlexDirection.Column, FlexAlignment.FlexEnd) => (e.X ?? 0) + (p.Width ?? 0) - (e.Width ?? 0 / 2) - (p.PaddingLeft ?? 0) - (p.PaddingRight ?? 0),
                (FlexPosition.Absolute, FlexDirection.Column, FlexAlignment.Center) => (e.X ?? 0) + (p.Width ?? 0) / 2 - (e.Width ?? 0 / 2),
                (FlexPosition.Absolute, FlexDirection.Column, FlexAlignment.FlexEnd) => (e.X ?? 0) + (p.Width ?? 0) - (e.Width ?? 0 / 2) - (p.PaddingLeft ?? 0) - (p.PaddingRight ?? 0),
                _ => e.X
            };
            if (p.FlexDirection == FlexDirection.Column && e.AlignSelf == FlexAlignment.Stretch)
                e.Width = (p.Width ?? 0) + (p.PaddingLeft ?? 0) + (p.PaddingRight ?? 0);
        }
    }


    void ResolveSpaceDistribution(FlexNode element, out ViewNumber availableWidth, out ViewNumber availableHeight, out ViewNumber totalFlex, out ViewNumber totalShrink, ref int childrenCount)
    {
        // Distribute space
        totalFlex = 0;
        totalShrink = 0;
        availableWidth = element.Value.Width ?? 0;
        availableHeight = element.Value.Height ?? 0;
        if (element.Value is BoxElement e)
        {
            foreach (var child in Adjacency[element])
            {
                if (child.Value is BoxElement c && c.Position == FlexPosition.Relative)
                {
                    childrenCount += 1;
                    if (e.FlexDirection == FlexDirection.Row && c.Grow >= 0)
                    {
                        availableWidth -= (c.Width ?? 0) +
                            (c.MarginLeft, c.MarginRight) switch
                            {
                                (ViewNumber ml, ViewNumber mr) => ml + mr,
                                (ViewNumber ml, null) => ml,
                                (null, ViewNumber mr) => mr,
                                (_, _) => 0

                            };
                    }
                    if (e.FlexDirection == FlexDirection.Column && c.Grow >= 0)
                        availableHeight -= (c.Height ?? 0) +
                            (c.MarginTop, c.MarginBottom) switch
                            {
                                (ViewNumber mt, ViewNumber mr) => mt + mr,
                                (ViewNumber mt, null) => mt,
                                (null, ViewNumber mb) => mb,
                                (_, _) => 0

                            };

                    if (e.FlexDirection == FlexDirection.Row && c.Grow > 0)
                        totalFlex += c.Grow ?? 0;
                    if (e.FlexDirection == FlexDirection.Column && c.Grow > 0)
                        totalFlex += c.Grow ?? 0;

                    if (e.FlexDirection == FlexDirection.Row && c.Shrink > 0)
                        totalShrink += c.Shrink ?? 0;
                    if (e.FlexDirection == FlexDirection.Column && c.Shrink > 0)
                        totalShrink += c.Shrink ?? 0;
                }



            }

            availableWidth -=
                (e.PaddingLeft ?? 0) +
                (e.PaddingRight ?? 0) +
                (e.FlexDirection == FlexDirection.Row &&
                e.JustifyContent != JustifyContent.SpaceBetween &&
                e.JustifyContent != JustifyContent.SpaceAround &&
                e.JustifyContent != JustifyContent.SpaceEvenly
                    ? (childrenCount - 1) * (e.Gap ?? 0)
                    : 0);
            availableHeight -=
                (e.PaddingTop ?? 0) +
                (e.PaddingBottom ?? 0) +
                (e.FlexDirection == FlexDirection.Column &&
                e.JustifyContent != JustifyContent.SpaceBetween &&
                e.JustifyContent != JustifyContent.SpaceAround &&
                e.JustifyContent != JustifyContent.SpaceEvenly
                    ? (childrenCount - 1) * (e.Gap ?? 0)
                    : 0);

            if (e.FlexWrap == FlexWrap.NoWrap)
            {
                foreach (var child in Adjacency[element])
                {
                    if (child.Value is BoxElement c)
                    {
                        if (e.JustifyContent != null && (!e.JustifyContent.Value.ToString().StartsWith("Space") || totalFlex > 0))
                        {
                            if (availableWidth >= 0)
                            {
                                if (e.FlexDirection == FlexDirection.Row && c.Grow > 0)
                                {
                                    c.Width = (c.Width ?? 0) + (c.Grow ?? 0) / totalFlex * availableWidth;
                                }
                            }
                            else
                            {
                                if (e.FlexDirection == FlexDirection.Row && c.Shrink > 0)
                                {
                                    c.Width = (c.Width ?? 0) - (c.Shrink ?? 0) / totalShrink * ViewNumber.Abs(availableWidth);
                                }
                            }
                            if (availableHeight >= 0)
                            {

                                if (e.FlexDirection == FlexDirection.Column && c.Grow > 0)
                                {
                                    c.Height = (c.Height ?? 0) + (c.Grow ?? 0) / totalShrink * availableHeight;
                                }
                            }
                            else
                            {
                                if (e.FlexDirection == FlexDirection.Column && c.Shrink > 0)
                                {
                                    c.Height = (c.Height ?? 0) - (c.Shrink ?? 0) / totalFlex * ViewNumber.Abs(availableHeight);
                                }
                            }
                        }
                        else
                        {
                            if (availableWidth < 0)
                            {
                                if (e.FlexDirection == FlexDirection.Row && c.Shrink > 0)
                                {
                                    c.Width = (c.Width ?? 0) - (c.Shrink ?? 0) / totalShrink * ViewNumber.Abs(availableWidth);
                                }
                            }
                            if (availableHeight < 0)
                            {
                                if (e.FlexDirection == FlexDirection.Column && c.Shrink > 0)
                                {
                                    c.Height = (c.Height ?? 0) - (c.Shrink ?? 0) / totalFlex * ViewNumber.Abs(availableHeight);
                                    availableHeight = 0;
                                }
                            }
                        }
                    }
                }
                if (availableWidth < 0)
                    availableWidth = 0;
                if (availableHeight < 0)
                    availableHeight = 0;
            }
            else 
            {
                // availableWidth = 0;
                // availableHeight = 0;
                // foreach (var child in Adjacency[element])
                // {
                    
                // }
            }
        }
    }

    void ResolveJustifyContent(FlexNode element, in ViewNumber availableWidth, in ViewNumber availableHeight, in ViewNumber totalFlex, in ViewNumber totalShrink, ref int childrenCount)
    {

        if (element.Value is BoxElement e)
        {
            var x = (e.X ?? 0) + (e.PaddingLeft ?? 0);
            var y = (e.Y ?? 0) + (e.PaddingTop ?? 0);
            // Justify content
            if (e.FlexDirection == FlexDirection.Row)
            {
                x += e.JustifyContent switch
                {
                    JustifyContent.Center => availableWidth / 2,
                    JustifyContent.FlexEnd => availableWidth,
                    _ => 0
                };
            }
            if (e.FlexDirection == FlexDirection.Column)
            {
                y += e.JustifyContent switch
                {
                    JustifyContent.Center => availableHeight / 2,
                    JustifyContent.FlexEnd => availableHeight,
                    _ => 0
                };
            }
            if (
                totalFlex == 0
                &&
                (
                    e.JustifyContent == JustifyContent.SpaceBetween
                    || e.JustifyContent == JustifyContent.SpaceAround
                    || e.JustifyContent == JustifyContent.SpaceEvenly
                )
            )
            {
                var count =
                    childrenCount +
                    (e.JustifyContent == JustifyContent.SpaceBetween
                    ? -1
                    : e.JustifyContent == JustifyContent.SpaceEvenly
                    ? 1
                    : 0);
                var horizontalGap = availableWidth / count;
                var verticalGap = availableHeight / count;

                foreach (var child in Adjacency[element])
                {
                    if (e.FlexDirection == FlexDirection.Row)
                    {
                        child.Value.X =
                            (child.Value.X ?? 0) +
                            x +
                            (
                                e.JustifyContent == JustifyContent.SpaceBetween
                                ? 0
                                : e.JustifyContent == JustifyContent.SpaceAround
                                ? horizontalGap / 2
                                : horizontalGap
                            );
                    }
                    else
                    {

                        child.Value.Y =
                            (child.Value.Y ?? 0) +
                            y +
                            (
                                e.JustifyContent == JustifyContent.SpaceBetween
                                ? 0
                                : e.JustifyContent == JustifyContent.SpaceAround
                                ? verticalGap / 2
                                : verticalGap
                            );
                    }

                    if (e.FlexDirection == FlexDirection.Row)
                        x += (child.Value.Width ?? 0) + horizontalGap;
                    if (e.FlexDirection == FlexDirection.Column)
                        y += (child.Value.Height ?? 0) + verticalGap;

                }
            }
            else
            {
                foreach (var child in Adjacency[element])
                {
                    if (child.Value is BoxElement c)
                    {
                        if (c.Position == FlexPosition.Absolute && c.Display == "none")
                            continue;
                        if (e.FlexDirection == FlexDirection.Row)
                        {
                            c.X = x;
                            x +=
                                (c.Width ?? 0) + (e.Gap ?? 0) +
                                (c.MarginLeft, c.MarginRight) switch
                                {
                                    (ViewNumber ml, ViewNumber mr) => ml + mr,
                                    (ViewNumber ml, null) => ml,
                                    (null, ViewNumber mr) => mr,
                                    (_, _) => 0

                                };
                        }
                        else
                        {
                            c.X += x;
                        }
                        if (e.FlexDirection == FlexDirection.Column)
                        {
                            c.Y = x;
                            y += (c.Height ?? 0) + (e.Gap ?? 0) +
                                (c.MarginTop, c.MarginBottom) switch
                                {
                                    (ViewNumber mt, ViewNumber mb) => mt + mb,
                                    (ViewNumber mt, null) => mt,
                                    (null, ViewNumber mb) => mb,
                                    (_, _) => 0
                                };
                        }
                        else
                        {
                            c.Y += y;
                        }
                    }
                }
            }

        }
    }
    private void ResolveAlignItems(FlexNode element)
    {
        if (element.Value is BoxElement e)
            foreach (var child in Adjacency[element])
            {
                if (child.Value is BoxElement c)
                {
                    if (c.Position == FlexPosition.Absolute)
                        continue;

                    if (e.FlexDirection == FlexDirection.Row)
                    {
                        if (e.AlignItems == FlexAlignment.Center)
                        {
                            c.Y =
                              e.Y + (e.Height ?? 0) / 2 - (c.Height ?? 0) / 2;
                        }

                        if (e.AlignItems == FlexAlignment.FlexEnd)
                        {
                            c.Y =
                              e.Y +
                              (e.Height ?? 0) -
                              (c.Height ?? 0) -
                              (e.PaddingBottom ?? 0);
                        }

                        if (e.AlignItems == FlexAlignment.Stretch)
                        {
                            c.Height =
                              e.Height - (e.PaddingTop ?? 0) - (e.PaddingBottom ?? 0);
                        }
                    }
                    if (e.FlexDirection == FlexDirection.Column)
                    {
                        if (e.AlignItems == FlexAlignment.Center)
                        {
                            c.X =
                              e.X + (e.Width ?? 0) / 2 - (c.Width ?? 0) / 2;
                        }

                        if (e.AlignItems == FlexAlignment.FlexEnd)
                        {
                            c.X =
                              e.X +
                              (e.Width ?? 0) -
                              (c.Width ?? 0) -
                              (e.PaddingRight ?? 0);
                        }

                        if (e.AlignItems == FlexAlignment.Stretch)
                        {
                            c.Width =
                              (e.Width ?? 0) - (e.PaddingLeft ?? 0) - (e.PaddingRight ?? 0);
                        }
                    }

                }
            }
    }
}