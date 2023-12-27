using System.Collections;

namespace SoftTouch.UI.Flexbox;


public class QueueNode<T>(T data, QueueNode<T>? prev = null, QueueNode<T>? next = null)
{
    public T Data { get; set; } = data;
    public QueueNode<T>? Prev { get; set; } = prev;
    public QueueNode<T>? Next { get; set; } = next;
}

public class FlexQueue<T>()
{
    List<QueueNode<T>> queue = new();

    public int Count => queue.Count;

    public void Enqueue(T value)
    {
        var node = new QueueNode<T>(value);

        var last = queue.LastOrDefault();
        if (last != null)
        {
            last.Next = node;
            node.Prev = last;
        }
        queue.Insert(0, node);
    }

    public T? DequeueFront()
    {
        if(queue.Count > 0)
        {
            var deq = queue[^1];
            queue.RemoveAt(queue.Count - 1);
            return deq.Data;
        }
        return default;
    }
    public T? Dequeue()
    {
        if(queue.Count > 0)
        {
            var deq = queue[0];
            if (deq.Prev != null)
                deq.Prev.Next = null;
            return deq.Data;
        }
        return default;
    }
    public Enumerator GetEnumerator() => new(queue);

    public ref struct Enumerator(List<QueueNode<T>> queue)
    {
        List<QueueNode<T>>.Enumerator queue = queue.GetEnumerator();

        public T Current => queue.Current.Data;
        public bool MoveNext() => queue.MoveNext();
    }
}