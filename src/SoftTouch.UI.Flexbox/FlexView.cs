namespace SoftTouch.UI.Flexbox;

public abstract record FlexView
{
    public static implicit operator FlexView(TextElement text) => new TextView(text);
    public static implicit operator FlexView(BoxElement box) => new BoxView(box, []);
    public static implicit operator FlexView((BoxElement, List<FlexView>) box) => new BoxView(box.Item1, box.Item2);
    public static implicit operator FlexView(ValueTuple<string, int, string> text) => new TextView(new(text.Item1, text.Item2, text.Item3));
    public static implicit operator FlexView(string text) => new TextView(new(text));
}

public record BoxView(BoxElement Box, List<FlexView> Children) : FlexView;
public record TextView(TextElement Text) : FlexView;