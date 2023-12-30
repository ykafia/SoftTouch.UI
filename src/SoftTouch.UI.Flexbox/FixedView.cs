namespace SoftTouch.UI.Flexbox;

public abstract class FlexElement
{
    public ViewNumber? X { get; set; } = 0;
    public ViewNumber? Y { get; set; } = 0;
    public ViewNumber? Width { get; set; }
    public ViewNumber? Height { get; set; }
    public int ZIndex { get; set; }
    public string? BackgroundColor { get; set; }
}

public class TextElement(string text = "", ViewNumber? fontSize = null, string color = "#000") : FlexElement
{
    public string Text { get; set; } = text;
    public ViewNumber FontSize { get; set; } = fontSize ?? 12;
    public string Color { get; set; } = color;

    public override string ToString()
    {
        return $"[color : {Color}, size : {FontSize}] {Text}";
    }
}

public class BoxElement : FlexElement
{
    public FlexDirection? FlexDirection { get; set; }
    public JustifyContent? JustifyContent { get; set; }
    public FlexAlignment? AlignItems { get; set; }
    public FlexAlignment? AlignSelf { get; set; }
    public ViewNumber? Flex { get; set; }
    public ViewPosition Position { get; set; }
    public ViewNumber? Gap { get; set; }
    public string? Display { get; set; }
    public ViewNumber? Top { get; set; }
    public ViewNumber? Left { get; set; }
    public ViewNumber? Right { get; set; }
    public ViewNumber? Bottom { get; set; }
    public ViewNumber? Padding { get; set; }
    public ViewNumber? PaddingHorizontal { get; set; }
    public ViewNumber? PaddingVertical { get; set; }
    public ViewNumber? PaddingLeft { get; set; }
    public ViewNumber? PaddingRight { get; set; }
    public ViewNumber? PaddingTop { get; set; }
    public ViewNumber? PaddingBottom { get; set; }
    public ViewNumber? Margin { get; set; }
    public ViewNumber? MarginHorizontal { get; set; }
    public ViewNumber? MarginVertical { get; set; }
    public ViewNumber? MarginLeft { get; set; }
    public ViewNumber? MarginRight { get; set; }
    public ViewNumber? MarginTop { get; set; }
    public ViewNumber? MarginBottom { get; set; }


    public override string ToString()
    {
        return $"[color : {BackgroundColor}] Box";
    }
}


