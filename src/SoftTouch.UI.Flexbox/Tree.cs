﻿namespace SoftTouch.UI.Flexbox;

public abstract class Tree()
{
    public Tree? Parent { get; set; }
    public Tree? Next { get; set; }
    public Tree? Prev { get; set; }
    public Tree? FirstChild { get; set; }
    public Tree? LastChild { get; set; }
}

public class Tree<T>() : Tree
    where T : FixedView
{
    public T? Value { get; set; }
    
    public Tree<TView> AddChild<TView>(Tree<TView> node)
        where TView : FixedView
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
