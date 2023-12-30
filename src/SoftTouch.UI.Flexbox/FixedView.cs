namespace SoftTouch.UI.Flexbox;

public abstract class FixedView
{
    public ViewNumber? X { get; set; }
    public ViewNumber? Y { get; set; }
    public ViewNumber? Width { get; set; }
    public ViewNumber? Height { get; set; }
    public int ZIndex { get; set; }
    public string? BackgroundColor { get; set; }
}

public class TextView : FixedView
{
    public string Text { get; set; } = "";
    public ViewNumber FontSize { get; set; }
    public string Color { get; set; } = "#000";
}

public class BoxView : FixedView
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
}


