﻿using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.Marshalling;

namespace SoftTouch.UI.Flexbox;


public sealed record FlexNode(FlexStyle Value, FlexNode Parent, FlexTree Tree)
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
    public Dictionary<FlexStyle, FlexNode> Lookup { get; } = [];
    public Dictionary<FlexNode, List<FlexNode>> Adjacency { get; } = [];
    public ReverseOrderer ReverseOrder => new(this);


    public FlexTree(BoxView view)
    {
        Root = new(view.Box, null!, this);
        Lookup[Root.Value] = Root;
        Adjacency[Root] = [];
        RecurseFill(view);
    }
    void RecurseFill(FlexView view, FlexStyle? parent = null)
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
            var element = forwardQueue.Dequeue() ?? throw new NullReferenceException();
            var childrenCount = 0;


            if (element.Value is BoxElement e)
            {
                var parent = element.Parent?.Value;
                if (parent is not null && element.Parent is not null)
                {
                    if (e.Grow < 0)
                        throw new Exception("Flex value cannot be negative");

                    // Resolving flex properties

                    ResolveFlexProperties(element, element.Parent);


                    ResolveAlignSelf(element, element.Parent);

                    // setting up the percentage size 
                    foreach (var child in Adjacency[element])
                    {
                        if (child.Value.Width is not null && child.Value.Width?.Kind == ViewNumberKind.Percentage)
                            child.Value.Width = child.Value.Width?.Percentage * element.Value.Width;
                        if (child.Value.Height is not null && child.Value.Height?.Kind == ViewNumberKind.Percentage)
                            child.Value.Height = child.Value.Height?.Percentage * element.Value.Height;
                    }
                    // ZIndex
                    element.Value.ZIndex = parent?.ZIndex ?? 0;
                }
                
                ResolveSpaceDistribution(element, out var availableWidth, out var availableHeight, out var totalFlex, out var totalShrink, ref childrenCount);
                e.X += e.MarginLeft ?? 0;
                e.Y += e.MarginTop ?? 0;


                // Determine positions.

                ResolveJustifyContent(element, in availableWidth, in availableHeight, in totalFlex, in totalShrink, ref childrenCount);

                ResolveAlignItems(element);


                e.X = Math.Round((double)(e.X ?? 0));
                e.Y = Math.Round((double)(e.Y ?? 0));
                e.Width = Math.Round((double)(e.Width ?? 0));
                e.Height = Math.Round((double)(e.Height ?? 0));
            }
        }

    }


    public IEnumerator<FlexStyle> GetEnumerator() => BreadthFirstOrder().OrderBy(x => x.ZIndex).GetEnumerator();

    public IEnumerable<FlexStyle> BreadthFirstOrder()
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
        public readonly IEnumerator<FlexStyle> GetEnumerator() => tree.BreadthFirstOrder().OrderBy(x => x.ZIndex).Reverse().GetEnumerator();
    }
}


