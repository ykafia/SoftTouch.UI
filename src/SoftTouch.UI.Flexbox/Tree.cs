using System.Diagnostics.CodeAnalysis;

namespace SoftTouch.UI.Flexbox;


public sealed record FlexNode(FixedView Value, FlexNode Parent, FlexTree Tree);

public sealed class FlexTree
{
    public FlexNode? Root { get; private set; }
    public Dictionary<FixedView, FlexNode> Lookup { get; } = [];
    public Dictionary<FlexNode, List<FlexNode>> Adjacency { get; } = [];

    public void AddChild(FixedView? parent, FixedView child)
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

    public IEnumerator<FixedView> GetEnumerator() => BreadthFirstOrder().OrderBy(x => x.ZIndex).GetEnumerator();

    public IEnumerable<FixedView> BreadthFirstOrder()
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
}


