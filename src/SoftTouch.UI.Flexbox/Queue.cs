using System.Collections;

namespace SoftTouch.UI.Flexbox;


public class FlexQueue()
{
    List<FlexNode> queue = [];

    public int Count => queue.Count;

    public FlexQueue(FlexQueue other) : this()
    {
        queue = new(other.queue);
    }

    public void Enqueue(FlexNode value)
    {
        queue.Insert(0, value);
    }

    public FlexNode? DequeueFront()
    {
        if(queue.Count > 0)
        {
            var deq = queue[^1];
            queue.RemoveAt(queue.Count - 1);
            return deq;
        }
        return null;
    }
    public FlexNode? Dequeue()
    {
        if(queue.Count > 0)
        {
            var deq = queue[0];
            queue.RemoveAt(0);
            return deq;
        }
        return null;
    }
    public List<FlexNode>.Enumerator GetEnumerator() => queue.GetEnumerator();

}