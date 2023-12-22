namespace SoftTouch.UI.Flexbox;

public abstract class FixedView
{
    public ViewNumber X { get; set; }
    public ViewNumber Y { get; set; }
    public ViewNumber Width { get; set; }
    public ViewNumber Height { get; set; }
    public int ZIndex { get; set; }
    public string? BackgroundColor { get; set; }
}

public class TextView : FixedView
{
    public TextStyle TextStyle { get; set; }
}

public class BoxView : FixedView
{
    public ViewStyle? ViewStyle { get; set; }
}


