namespace SoftTouch.UI.Flexbox;

public class Tree<T>()
{
    public T? Value { get; set; }
    public Tree<T>? Parent { get; set; }
    public Tree<T>? Next { get; set; }
    public Tree<T>? Prev { get; set; }
    public Tree<T>? FirstChild { get; set; }
    public Tree<T>? LastChild { get; set; }

    public Tree<T> AddChild(Tree<T> node)
    {
        if(FirstChild is null)
        {
            FirstChild = node;
            LastChild = node;
        }
        else{
            if(LastChild is null)
                throw new Exception("Last child must be set");
            node.Prev = LastChild;
            LastChild.Next = node;
            LastChild = node;
        }
        return node;
    }
}
