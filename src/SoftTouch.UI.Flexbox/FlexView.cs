namespace SoftTouch.UI.Flexbox;

public abstract record FlexView
{
    public static implicit operator FlexView(TextStyle text) => new TextView(text);
    public static implicit operator FlexView(BoxElement box) => new BoxView(box, []);
    public static implicit operator FlexView((BoxElement, List<FlexView>) box) => new BoxView(box.Item1, box.Item2);
    public static implicit operator FlexView(in ValueTuple<string, string, int, string> text) => new TextView(new(text: text.Item1, fontFamily: text.Item2, fontSize: text.Item3, color: text.Item4));
    public static implicit operator FlexView(string text) => new TextView(new(text));
}

public record BoxView(BoxElement Box, List<FlexView> Children) : FlexView;
public record TextView(TextStyle Text) : FlexView;