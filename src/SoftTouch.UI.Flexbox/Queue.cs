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
    Queue<QueueNode<T>> queue = new();

    public void Enqueue(T value)
    {
        var node = new QueueNode<T>(value);

        var last = queue.Last();
        if (last != null)
        {
            last.Next = node;
            node.Prev = last;
        }
        queue.Enqueue(node);
    }

    public void Dequeue()
    {
        var deq = queue.Dequeue();
        if(deq.Prev != null)
            deq.Prev.Next = null;
    }
}