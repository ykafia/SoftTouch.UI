using System.ComponentModel;
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

public sealed partial class FlexTree
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

    public void Update()
    {
        // First pass
        var secondPass = ToQueue();
        var forwardQueue = new FlexQueue();

        // Second pass
        PrecomputeParentSizes(secondPass, forwardQueue);


        // Third pass

        while (forwardQueue.Count > 0)
        {
            var e = forwardQueue.Dequeue() ?? throw new NullReferenceException();
            ViewNumber totalFlex = 0;
            var childrenCount = 0;

            var parent = e.Parent?.Value;
            if (parent is not null && e.Parent is not null)
            {
                if (e.Value is BoxElement ebv)
                {
                    if (ebv.Grow < 0)
                        throw new Exception("Flex value cannot be negative");

                    // Resolving flex properties

                    ResolveFlexProperties(e, e.Parent);


                    ResolveAlignSelf(e, e.Parent);

                    // setting up the percentage size 
                    foreach (var child in Adjacency[e])
                    {
                        if (child.Value.Width is not null && child.Value.Width?.Kind == ViewNumberKind.Percentage)
                            child.Value.Width = child.Value.Width?.Percentage * e.Value.Width;
                        if (child.Value.Height is not null && child.Value.Height?.Kind == ViewNumberKind.Percentage)
                            child.Value.Height = child.Value.Height?.Percentage * e.Value.Height;
                    }
                    // ZIndex
                    e.Value.ZIndex = parent?.ZIndex ?? 0;

                    ResolveSpaceDistribution(e, e.Parent, totalFlex, out ViewNumber availableWidth, out ViewNumber availableHeight, ref childrenCount);

                    ebv.X += ebv.MarginLeft ?? 0;
                    ebv.Y += ebv.MarginTop ?? 0;

                    // Determine positions.
                    var x = ebv.X + (ebv.PaddingLeft ?? 0);
                    var y = ebv.Y + (ebv.PaddingTop ?? 0);

                    ResolveJustifyContent(e, e.Parent, in availableWidth, in availableHeight, ref childrenCount);

                    ResolveAlignItems(e, e.Parent);

                    
                    ebv.X = Math.Round((double)ebv.X);
                    ebv.Y = Math.Round((double)ebv.Y);
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


