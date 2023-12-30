using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.Marshalling;

namespace SoftTouch.UI.Flexbox;


public sealed record FlexNode(FlexElement Value, FlexNode Parent, FlexTree Tree)
{
    public FlexNode? FirstChild => Tree.Adjacency[this].Count > 0 ? Tree.Adjacency[this][0] : null;
    public FlexNode? LastChild => Tree.Adjacency[this].Count > 0 ? Tree.Adjacency[this][^1] : null;
    public FlexNode? Previous
    {
        get
        {
            var idx = Tree.Adjacency[Parent].IndexOf(this);
            return idx > 1 ? Tree.Adjacency[Parent][idx - 1] : null;
        }
    }
    public FlexNode? Next
    {
        get
        {
            var idx = Tree.Adjacency[Parent].IndexOf(this);
            return idx < Tree.Adjacency[Parent].Count - 1 ? Tree.Adjacency[Parent][idx + 1] : null;
        }
    }

    public override string ToString()
    {
        return Value.ToString() ?? "";
    }
}

public sealed class FlexTree
{
    public FlexNode? Root { get; private set; }
    public Dictionary<FlexElement, FlexNode> Lookup { get; } = [];
    public Dictionary<FlexNode, List<FlexNode>> Adjacency { get; } = [];
    public ReverseOrderer ReverseOrder => new(this);


    public FlexTree(BoxView view)
    {
        Root = new(view.Box, null!, this);
        Lookup[Root.Value] = Root;
        Adjacency[Root] = [];
        RecurseFill(view);
    }
    void RecurseFill(FlexView view, FlexElement? parent = null)
    {
        if (view is BoxView bv)
        {
            if (bv.Box == Root?.Value)
                foreach (var child in bv.Children)
                    RecurseFill(child, bv.Box);
            else if (parent != null)
            {
                var node = new FlexNode(bv.Box, Lookup[parent], this);
                Lookup[node.Value] = node;
                Adjacency[node] = [];
                Adjacency[Lookup[parent]].Add(node);
                foreach (var child in bv.Children)
                    RecurseFill(child, node.Value);
            }
        }
        else if (view is TextView tv && parent != null)
        {
            var node = new FlexNode(tv.Text, Lookup[parent], this);
            Lookup[node.Value] = node;
            Adjacency[node] = [];
            Adjacency[Lookup[parent]].Add(node);
        }
    }

    public void AddChild(FlexElement? parent, FlexElement child)
    {
        if (parent is not null && child != null)
        {
            var childNode = new FlexNode(child, Lookup[parent], this);
            Lookup[child] = childNode;
            Adjacency[Lookup[parent]].Add(childNode);
            Adjacency[childNode] = [];
            child.ZIndex = parent.ZIndex + 1;

        }
        else if (parent is null && Root == null && child != null)
        {
            Root = new(child, null!, this);
            Adjacency[Root] = [];
            Lookup[child] = Root;
        }
        else
            throw new NotImplementedException();
    }

    public void Update()
    {
        // First pass
        var secondPass = ToQueue();
        var forwardQueue = new FlexQueue();

        // Second pass
        while (secondPass.Count > 0)
        {
            var e = secondPass.DequeueFront() ?? throw new NullReferenceException();
            forwardQueue.Enqueue(e);

            // get them value of e

            if (e.Value.Width is null)
            {
                var childrenCount = 0;

                foreach (var child in Adjacency[e])
                {
                    if (child.Value is BoxElement cbv)
                    {
                        if (cbv.Width is not null)
                        {
                            if (cbv.FlexDirection == FlexDirection.Row && cbv.Position == ViewPosition.Relative)
                                e.Value.Width = cbv.Width + cbv.MarginLeft + cbv.MarginRight;

                            if (cbv.FlexDirection == FlexDirection.Column && cbv.Position == ViewPosition.Relative)
                                e.Value.Width = ViewNumber.MaxMagnitude(
                                    e.Value.Width ?? 0,
                                    cbv.Width ?? 0 + cbv.MarginLeft ?? 0 + cbv.MarginRight ?? 0
                                );
                        }
                        if (cbv.Position == ViewPosition.Relative)
                            childrenCount += 1;
                    }
                }

                if (e.Value is BoxElement ebv)
                    ebv.Width =
                        ebv.PaddingLeft ?? 0
                        + ebv.PaddingRight ?? 0
                        + (ebv.FlexDirection == FlexDirection.Row ? (childrenCount - 1) * ebv.Gap : 0);
            }

            if (e.Value.Height is null)
            {
                var childrenCount = 0;

                foreach (var child in Adjacency[e])
                {
                    if (child.Value is BoxElement cbv)
                    {
                        if (cbv.Width is not null)
                        {
                            if (cbv.FlexDirection == FlexDirection.Column && cbv.Position == ViewPosition.Relative)
                                e.Value.Height = cbv.Height + cbv.MarginTop + cbv.MarginBottom;

                            if (cbv.FlexDirection == FlexDirection.Row && cbv.Position == ViewPosition.Relative)
                                e.Value.Height = ViewNumber.MaxMagnitude(
                                    e.Value.Height ?? 0,
                                    cbv.Height ?? 0 + cbv.MarginTop ?? 0 + cbv.MarginBottom ?? 0
                                );
                        }
                        if (cbv.Position == ViewPosition.Relative)
                            childrenCount += 1;
                    }
                }

                if (e.Value is BoxElement ebv)
                    ebv.Height =
                        ebv.PaddingTop ?? 0
                        + ebv.PaddingBottom ?? 0
                        + (ebv.FlexDirection == FlexDirection.Column ? (childrenCount - 1) * ebv.Gap : 0);
            }
        }


        // Third pass

        while (forwardQueue.Count > 0)
        {
            var e = forwardQueue.DequeueFront() ?? throw new NullReferenceException();
            ViewNumber totalFlex = 0;
            var childrenCount = 0;

            if (e.Parent != null)
            {
                var parent = e.Parent;
                var parentWidth = parent.Value.Width ?? 0;
                var parentHeight = parent.Value.Height ?? 0;

                if (e.Value is BoxElement ebv && parent.Value is BoxElement pbv)
                {
                    if (ebv.Flex < 0)
                        throw new Exception("Flex value cannot be negative");

                    // Resolving flex properties
                    // For left right first
                    if (ebv.Width is not null && ebv.Width?.Kind == ViewNumberKind.Percentage)
                        ebv.Width = ebv.Width?.Percentage * pbv.Width;
                    if (
                        ebv.Left is not null
                        && ebv.Right is not null
                        && ebv.Width is null
                    )
                    {
                        ebv.X = (pbv.X ?? 0) + ebv.Left;
                        ebv.Width = pbv.Width - ebv.Left - ebv.Right;
                    }
                    else if (ebv.Left is not null)
                    {
                        ebv.X = ebv.Position == ViewPosition.Absolute ?
                            (pbv.X ?? 0) + ebv.Left
                            : ebv.X + ebv.Left;
                    }
                    else if (ebv.Right is not null)
                    {
                        ebv.X = ebv.Position == ViewPosition.Absolute ?
                            (pbv.X ?? 0) + pbv.Width - ebv.Right - ebv.Width
                            : pbv.X - ebv.Right;
                    }
                    else if (ebv.Position == ViewPosition.Absolute)
                    {
                        ebv.X = pbv.X ?? 0;
                    }
                    // Then top bottom

                    if (ebv.Height is not null && ebv.Height?.Kind == ViewNumberKind.Percentage)
                        ebv.Height = ebv.Height?.Percentage * pbv.Height;
                    if (
                        ebv.Top is not null
                        && ebv.Bottom is not null
                        && ebv.Height is null
                    )
                    {
                        ebv.Y = (pbv.Y ?? 0) + ebv.Top;
                        ebv.Height = pbv.Height - ebv.Top - ebv.Bottom;
                    }
                    else if (ebv.Top is not null)
                    {
                        ebv.Y = ebv.Position == ViewPosition.Absolute ?
                            (pbv.Y ?? 0) + ebv.Top
                            : ebv.Y + ebv.Top;
                    }
                    else if (ebv.Right is not null)
                    {
                        ebv.Y = ebv.Position == ViewPosition.Absolute ?
                            (pbv.Y ?? 0) + pbv.Height - ebv.Bottom - ebv.Height
                            : pbv.Y - ebv.Bottom;
                    }
                    else if (ebv.Position == ViewPosition.Absolute)
                    {
                        ebv.Y = pbv.Y ?? 0;
                    }


                    // Resolving Align self

                    if (ebv.Position == ViewPosition.Absolute)
                    {
                        if (pbv.FlexDirection == FlexDirection.Row)
                        {
                            if (ebv.AlignSelf == FlexAlignment.Center)
                                ebv.Y += ebv.Height / 2 - ebv.Height / 2;

                            if (ebv.AlignSelf == FlexAlignment.FlexEnd)
                                ebv.Y +=
                                    pbv.Height - ebv.Height - pbv.PaddingBottom - pbv.PaddingTop;

                            if (ebv.AlignSelf == FlexAlignment.Stretch)
                                ebv.Height =
                                    pbv.Height -
                                    pbv.PaddingBottom -
                                    pbv.PaddingTop;
                        }
                        if (pbv.FlexDirection == FlexDirection.Column)
                        {
                            if (ebv.AlignSelf == FlexAlignment.Center)
                                ebv.X += ebv.Width / 2 - ebv.Width / 2;

                            if (ebv.AlignSelf == FlexAlignment.FlexEnd)
                                ebv.X +=
                                    pbv.Width - ebv.Width - pbv.PaddingLeft - pbv.PaddingRight;

                            if (ebv.AlignSelf == FlexAlignment.Stretch)
                                ebv.Width =
                                    pbv.Width -
                                    pbv.PaddingLeft -
                                    pbv.PaddingRight;
                        }
                    }

                    // setting up the percentage size 
                    foreach (var child in Adjacency[e])
                    {
                        if (child.Value.Width is not null)
                            child.Value.Width = child.Value.Width?.Percentage * e.Value.Width;
                        if (child.Value.Height is not null)
                            child.Value.Height = child.Value.Height?.Percentage * e.Value.Height;
                    }
                    // ZIndex
                    e.Value.ZIndex = parent.Value.ZIndex;

                    // Distribute space

                    var availableWidth = e.Value.Width;
                    var availableHeight = e.Value.Height;

                    foreach (var child in Adjacency[e])
                    {
                        if (child.Value is BoxElement cbv)
                        {
                            if (cbv.Position == ViewPosition.Relative)
                                childrenCount += 1;
                            if (
                                ebv.FlexDirection == FlexDirection.Row
                                && cbv.Flex is null
                                && cbv.Position == ViewPosition.Relative
                            )
                            {
                                availableWidth -= cbv.Width;
                            }
                            if (
                                ebv.FlexDirection == FlexDirection.Column
                                && cbv.Flex is null
                                && cbv.Position == ViewPosition.Relative
                            )
                            {
                                availableHeight -= cbv.Height;
                            }

                            if (ebv.FlexDirection == FlexDirection.Row && ebv.Flex is not null)
                                totalFlex += cbv.Flex ?? 0;
                            if (ebv.FlexDirection == FlexDirection.Column && ebv.Flex is not null)
                                totalFlex += cbv.Flex ?? 0;
                        }
                    }

                    availableWidth -=
                        ebv.PaddingLeft +
                        ebv.PaddingRight +
                        (ebv.FlexDirection == FlexDirection.Row &&
                        ebv.JustifyContent != JustifyContent.SpaceBetween &&
                        ebv.JustifyContent != JustifyContent.SpaceAround &&
                        ebv.JustifyContent != JustifyContent.SpaceEvenly
                            ? (childrenCount - 1) * ebv.Gap
                            : 0);
                    availableHeight -=
                        ebv.PaddingTop +
                        ebv.PaddingBottom +
                        (ebv.FlexDirection == FlexDirection.Column &&
                        ebv.JustifyContent != JustifyContent.SpaceBetween &&
                        ebv.JustifyContent != JustifyContent.SpaceAround &&
                        ebv.JustifyContent != JustifyContent.SpaceEvenly
                            ? (childrenCount - 1) * ebv.Gap
                            : 0);
                    foreach (var child in Adjacency[e])
                    {
                        if (child.Value is BoxElement cbv)
                        {
                            if (ebv.FlexDirection == FlexDirection.Row)
                            {
                                if (
                                    cbv.Flex is not null
                                    && ebv.JustifyContent != JustifyContent.SpaceBetween
                                    && ebv.JustifyContent != JustifyContent.SpaceAround
                                    && ebv.JustifyContent != JustifyContent.SpaceEvenly
                                )
                                    cbv.Width = (cbv.Flex / totalFlex) * availableWidth;
                            }
                            if (ebv.FlexDirection == FlexDirection.Row)
                            {
                                if (
                                    cbv.Flex is not null
                                    && ebv.JustifyContent != JustifyContent.SpaceBetween
                                    && ebv.JustifyContent != JustifyContent.SpaceAround
                                    && ebv.JustifyContent != JustifyContent.SpaceEvenly
                                )
                                    cbv.Height = (cbv.Flex / totalFlex) * availableHeight;
                            }
                        }
                    }

                    ebv.X += ebv.MarginLeft;
                    ebv.Y += ebv.MarginTop;

                    // Determine positions.
                    var x = ebv.X + ebv.PaddingLeft;
                    var y = ebv.Y + ebv.PaddingTop;

                    if (ebv.FlexDirection == FlexDirection.Row)
                    {
                        x += ebv.JustifyContent switch
                        {
                            JustifyContent.Center => availableWidth / 2,
                            JustifyContent.FlexEnd => availableWidth,
                            _ => 0
                        };
                    }
                    if (ebv.FlexDirection == FlexDirection.Column)
                    {
                        y += ebv.JustifyContent switch
                        {
                            JustifyContent.Center => availableHeight / 2,
                            JustifyContent.FlexEnd => availableHeight,
                            _ => 0
                        };
                    }

                    if (
                        ebv.JustifyContent == JustifyContent.SpaceBetween
                        || ebv.JustifyContent == JustifyContent.SpaceAround
                        || ebv.JustifyContent == JustifyContent.SpaceEvenly
                    )
                    {
                        var count =
                            childrenCount +
                            (ebv.JustifyContent == JustifyContent.SpaceBetween
                            ? -1
                            : ebv.JustifyContent == JustifyContent.SpaceEvenly
                            ? 1
                            : 0);
                        var horizontalGap = availableWidth / count;
                        var verticalGap = availableHeight / count;

                        foreach (var child in Adjacency[e])
                        {

                            child.Value.X +=
                                ebv.JustifyContent == JustifyContent.SpaceBetween
                                ? 0
                                : ebv.JustifyContent == JustifyContent.SpaceAround
                                ? horizontalGap / 2
                                : horizontalGap;

                            child.Value.Y +=
                                ebv.JustifyContent == JustifyContent.SpaceBetween
                                ? 0
                                : ebv.JustifyContent == JustifyContent.SpaceAround
                                ? verticalGap / 2
                                : verticalGap;

                            if (ebv.FlexDirection == FlexDirection.Row)
                                x += child.Value.Width + horizontalGap;
                            if (ebv.FlexDirection == FlexDirection.Column)
                                y += child.Value.Height + verticalGap;

                        }
                    }
                    else
                    {
                        foreach (var child in Adjacency[e])
                        {
                            if (child.Value is BoxElement cbv)
                            {
                                if (cbv.Position == ViewPosition.Absolute && cbv.Display == "none")
                                    continue;
                                if (ebv.FlexDirection == FlexDirection.Row)
                                {
                                    cbv.X = x;
                                    x += cbv.Width + ebv.Gap;
                                }
                                else
                                {
                                    cbv.X += x;
                                }
                                if (ebv.FlexDirection == FlexDirection.Column)
                                {
                                    cbv.Y = x;
                                    y += cbv.Height + ebv.Gap;
                                }
                                else
                                {
                                    cbv.Y += y;
                                }
                            }
                        }
                    }

                    foreach (var child in Adjacency[e])
                    {
                        if (child.Value is BoxElement cbv)
                        {
                            if (cbv.Position == ViewPosition.Absolute)
                                continue;

                            if (ebv.FlexDirection == FlexDirection.Row)
                            {
                                if (ebv.AlignItems == FlexAlignment.Center)
                                {
                                    cbv.Y =
                                      ebv.Y + ebv.Height / 2 - cbv.Height / 2;
                                }

                                if (ebv.AlignItems == FlexAlignment.FlexEnd)
                                {
                                    cbv.Y =
                                      ebv.Y +
                                      ebv.Height -
                                      cbv.Height -
                                      ebv.PaddingBottom;
                                }

                                if (ebv.AlignItems == FlexAlignment.Stretch && cbv.Height is null)
                                {
                                    cbv.Height =
                                      ebv.Height - ebv.PaddingTop - ebv.PaddingBottom;
                                }
                            }
                            if (ebv.FlexDirection == FlexDirection.Column)
                            {
                                if (ebv.AlignItems == FlexAlignment.Center)
                                {
                                    cbv.X =
                                      ebv.X + ebv.Width / 2 - cbv.Width / 2;
                                }

                                if (ebv.AlignItems == FlexAlignment.FlexEnd)
                                {
                                    cbv.X =
                                      ebv.X +
                                      ebv.Width -
                                      cbv.Width -
                                      ebv.PaddingRight;
                                }

                                if (ebv.AlignItems == FlexAlignment.Stretch && cbv.Width is null)
                                {
                                    cbv.Width =
                                      ebv.Width - ebv.PaddingLeft - ebv.PaddingRight;
                                }
                            }

                        }
                    }
                    ebv.X = Math.Round((double)(ebv.X ?? 0));
                    ebv.Y = Math.Round((double)(ebv.Y ?? 0));
                    ebv.Width = Math.Round((double)(ebv.Width ?? 0));
                    ebv.Height = Math.Round((double)(ebv.Height ?? 0));
                }
            }
        }
    }


    public IEnumerator<FlexElement> GetEnumerator() => BreadthFirstOrder().OrderBy(x => x.ZIndex).GetEnumerator();

    public IEnumerable<FlexElement> BreadthFirstOrder()
    {
        if (Root == null)
        {
            yield break;
        }

        Queue<FlexNode> queue = new();
        queue.Enqueue(Root);

        while (queue.Count > 0)
        {
            FlexNode current = queue.Dequeue();
            yield return current.Value;

            foreach (FlexNode child in Adjacency[current])
            {
                queue.Enqueue(child);
            }
        }
    }

    public FlexQueue ToQueue()
    {
        if (Root == null)
            return new();
        var queue = new Queue<FlexNode>();
        var result = new FlexQueue();

        queue.Enqueue(Root);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            result.Enqueue(current);
            foreach (var child in Adjacency[current])
                queue.Enqueue(child);
        }
        return result;
    }


    public readonly ref struct ReverseOrderer(FlexTree tree)
    {
        readonly FlexTree tree = tree;
        public readonly IEnumerator<FlexElement> GetEnumerator() => tree.BreadthFirstOrder().OrderBy(x => x.ZIndex).Reverse().GetEnumerator();
    }
}


