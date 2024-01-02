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
                    else if (child.Value is TextElement ctv)
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
                    else if (child.Value is TextElement ctv)
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
        }
    }

    void ResolveAlignSelf(FlexNode element, FlexNode parent)
    {
        // Resolving Align self

        //



        // if (ebv.Position == ViewPosition.Absolute)
        // {
        //     if (parent is BoxElement pbv && pbv.FlexDirection == FlexDirection.Row)
        //     {
        //         if (ebv.AlignSelf == FlexAlignment.Center)
        //             ebv.Y += (ebv.Height ?? 0) / 2 - (ebv.Height ?? 0) / 2;

        //         if (ebv.AlignSelf == FlexAlignment.FlexEnd)
        //             ebv.Y +=
        //                 (parent?.Height ?? 0) - (ebv.Height ?? 0) - (pbv.PaddingBottom ?? 0) - (pbv.PaddingTop ?? 0);

        //         if (ebv.AlignSelf == FlexAlignment.Stretch)
        //             ebv.Height =
        //                 pbv.Height -
        //                 pbv.PaddingBottom -
        //                 pbv.PaddingTop;
        //     }
        //     if (parent is BoxElement pbv2 && pbv2.FlexDirection == FlexDirection.Column)
        //     {
        //         if (ebv.AlignSelf == FlexAlignment.Center)
        //             ebv.X += (ebv.Width ?? 0) / 2 - (ebv.Width ?? 0) / 2;

        //         if (ebv.AlignSelf == FlexAlignment.FlexEnd)
        //             ebv.X +=
        //                 (pbv2.Width ?? 0) - (ebv.Width ?? 0) - (pbv2.PaddingLeft ?? 0) - (pbv2.PaddingRight ?? 0);

        //         if (ebv.AlignSelf == FlexAlignment.Stretch)
        //             ebv.Width =
        //                 pbv2.Width -
        //                 pbv2.PaddingLeft -
        //                 pbv2.PaddingRight;
        //     }
        // }
    }


    void ResolveSpaceDistribution(FlexNode element, FlexNode parent, ViewNumber totalFlex, out ViewNumber availableWidth, out ViewNumber availableHeight, ref int childrenCount)
    {
        // Distribute space

        availableWidth = element.Value.Width ?? 0;
        availableHeight = element.Value.Height ?? 0;

        // foreach (var child in Adjacency[e])
        // {
        //     if (child.Value is BoxElement cbv)
        //     {
        //         if (cbv.Position == ViewPosition.Relative)
        //             childrenCount += 1;
        //         if (
        //             ebv.FlexDirection == FlexDirection.Row
        //             && cbv.Grow is null
        //             && cbv.Position == ViewPosition.Relative
        //         )
        //         {
        //             availableWidth -= cbv.Width ?? 0;
        //         }
        //         if (
        //             ebv.FlexDirection == FlexDirection.Column
        //             && cbv.Grow is null
        //             && cbv.Position == ViewPosition.Relative
        //         )
        //         {
        //             availableHeight -= cbv.Height ?? 0;
        //         }

        //         if (ebv.FlexDirection == FlexDirection.Row && cbv.Grow is not null)
        //             totalFlex += cbv.Grow ?? 0;
        //         if (ebv.FlexDirection == FlexDirection.Column && cbv.Grow is not null)
        //             totalFlex += cbv.Grow ?? 0;
        //     }
        // }

        // availableWidth -=
        //     (ebv.PaddingLeft ?? 0) +
        //     (ebv.PaddingRight ?? 0) +
        //     (ebv.FlexDirection == FlexDirection.Row &&
        //     ebv.JustifyContent != JustifyContent.SpaceBetween &&
        //     ebv.JustifyContent != JustifyContent.SpaceAround &&
        //     ebv.JustifyContent != JustifyContent.SpaceEvenly
        //         ? (childrenCount - 1) * (ebv.Gap ?? 0)
        //         : 0);
        // availableHeight -=
        //     (ebv.PaddingTop ?? 0) +
        //     (ebv.PaddingBottom ?? 0) +
        //     (ebv.FlexDirection == FlexDirection.Column &&
        //     ebv.JustifyContent != JustifyContent.SpaceBetween &&
        //     ebv.JustifyContent != JustifyContent.SpaceAround &&
        //     ebv.JustifyContent != JustifyContent.SpaceEvenly
        //         ? (childrenCount - 1) * (ebv.Gap ?? 0)
        //         : 0);
        // foreach (var child in Adjacency[e])
        // {
        //     if (child.Value is BoxElement cbv)
        //     {
        //         if (ebv.FlexDirection == FlexDirection.Row)
        //         {
        //             if (
        //                 cbv.Grow is not null
        //                 && ebv.JustifyContent != JustifyContent.SpaceBetween
        //                 && ebv.JustifyContent != JustifyContent.SpaceAround
        //                 && ebv.JustifyContent != JustifyContent.SpaceEvenly
        //             )
        //                 cbv.Width = (cbv.Grow / totalFlex) * availableWidth;
        //         }
        //         if (ebv.FlexDirection == FlexDirection.Column)
        //         {
        //             if (
        //                 cbv.Grow is not null
        //                 && ebv.JustifyContent != JustifyContent.SpaceBetween
        //                 && ebv.JustifyContent != JustifyContent.SpaceAround
        //                 && ebv.JustifyContent != JustifyContent.SpaceEvenly
        //             )
        //                 cbv.Height = cbv.Grow / totalFlex * availableHeight;
        //         }
        //     }
        // }
    }

    void ResolveJustifyContent(FlexNode element, FlexNode parent, in ViewNumber availableWidth, in ViewNumber availableHeight, ref int childrenCount)
    {

        // Justify content
        // if (ebv.FlexDirection == FlexDirection.Row)
        // {
        //     x += ebv.JustifyContent switch
        //     {
        //         JustifyContent.Center => availableWidth / 2,
        //         JustifyContent.FlexEnd => availableWidth,
        //         _ => 0
        //     };
        // }
        // if (ebv.FlexDirection == FlexDirection.Column)
        // {
        //     y += ebv.JustifyContent switch
        //     {
        //         JustifyContent.Center => availableHeight / 2,
        //         JustifyContent.FlexEnd => availableHeight,
        //         _ => 0
        //     };
        // }

        // if (
        //     ebv.JustifyContent == JustifyContent.SpaceBetween
        //     || ebv.JustifyContent == JustifyContent.SpaceAround
        //     || ebv.JustifyContent == JustifyContent.SpaceEvenly
        // )
        // {
        //     var count =
        //         childrenCount +
        //         (ebv.JustifyContent == JustifyContent.SpaceBetween
        //         ? -1
        //         : ebv.JustifyContent == JustifyContent.SpaceEvenly
        //         ? 1
        //         : 0);
        //     var horizontalGap = availableWidth / count;
        //     var verticalGap = availableHeight / count;

        //     foreach (var child in Adjacency[e])
        //     {

        //         child.Value.X +=
        //             x +
        //             (
        //                 ebv.JustifyContent == JustifyContent.SpaceBetween
        //                 ? 0
        //                 : ebv.JustifyContent == JustifyContent.SpaceAround
        //                 ? horizontalGap / 2
        //                 : horizontalGap
        //             );

        //         child.Value.Y +=
        //             y +
        //             (
        //                 ebv.JustifyContent == JustifyContent.SpaceBetween
        //                 ? 0
        //                 : ebv.JustifyContent == JustifyContent.SpaceAround
        //                 ? verticalGap / 2
        //                 : verticalGap
        //             );

        //         if (ebv.FlexDirection == FlexDirection.Row)
        //             x += (child.Value.Width ?? 0) + horizontalGap;
        //         if (ebv.FlexDirection == FlexDirection.Column)
        //             y += (child.Value.Height ?? 0) + verticalGap;

        //     }
        // }
        // else
        // {
        //     foreach (var child in Adjacency[e])
        //     {
        //         if (child.Value is BoxElement cbv)
        //         {
        //             if (cbv.Position == ViewPosition.Absolute && cbv.Display == "none")
        //                 continue;
        //             if (ebv.FlexDirection == FlexDirection.Row)
        //             {
        //                 cbv.X = x;
        //                 x += (cbv.Width ?? 0) + (ebv.Gap ?? 0);
        //             }
        //             else
        //             {
        //                 cbv.X += x;
        //             }
        //             if (ebv.FlexDirection == FlexDirection.Column)
        //             {
        //                 cbv.Y = x;
        //                 y += (cbv.Height ?? 0) + (ebv.Gap ?? 0);
        //             }
        //             else
        //             {
        //                 cbv.Y += y;
        //             }
        //         }
        //     }
        // }
    }
    private void ResolveAlignItems(FlexNode e, FlexNode parent)
    {
        // foreach (var child in Adjacency[e])
        // {
        //     if (child.Value is BoxElement cbv)
        //     {
        //         if (cbv.Position == ViewPosition.Absolute)
        //             continue;

        //         if (ebv.FlexDirection == FlexDirection.Row)
        //         {
        //             if (ebv.AlignItems == FlexAlignment.Center)
        //             {
        //                 cbv.Y =
        //                   ebv.Y + (ebv.Height ?? 0) / 2 - (cbv.Height ?? 0) / 2;
        //             }

        //             if (ebv.AlignItems == FlexAlignment.FlexEnd)
        //             {
        //                 cbv.Y =
        //                   ebv.Y +
        //                   (ebv.Height ?? 0) -
        //                   (cbv.Height ?? 0) -
        //                   (ebv.PaddingBottom ?? 0);
        //             }

        //             if (ebv.AlignItems == FlexAlignment.Stretch && cbv.Height is null)
        //             {
        //                 cbv.Height =
        //                   ebv.Height - ebv.PaddingTop - ebv.PaddingBottom;
        //             }
        //         }
        //         if (ebv.FlexDirection == FlexDirection.Column)
        //         {
        //             if (ebv.AlignItems == FlexAlignment.Center)
        //             {
        //                 cbv.X =
        //                   ebv.X + (ebv.Width ?? 0) / 2 - (cbv.Width ?? 0) / 2;
        //             }

        //             if (ebv.AlignItems == FlexAlignment.FlexEnd)
        //             {
        //                 cbv.X =
        //                   ebv.X +
        //                   (ebv.Width ?? 0) -
        //                   (cbv.Width ?? 0) -
        //                   (ebv.PaddingRight ?? 0);
        //             }

        //             if (ebv.AlignItems == FlexAlignment.Stretch && cbv.Width is null)
        //             {
        //                 cbv.Width =
        //                   (ebv.Width ?? 0) - (ebv.PaddingLeft ?? 0) - (ebv.PaddingRight ?? 0);
        //             }
        //         }

        //     }
        // }
    }
}