namespace SoftTouch.UI.Flexbox;

public abstract class FlexElement(
    string? id = null,
    ViewNumber? x = null,
    ViewNumber? y = null,
    ViewNumber? width = null,
    ViewNumber? height = null,
    int? zindex = null,
    string? backgroundColor = null
)
{
    public string? Id { get; set; } = id;
    public ViewNumber? X { get; set; } = x; 
    public ViewNumber? Y { get; set; } = y;
    public ViewNumber? Width { get; set; } = width;
    public ViewNumber? Height { get; set; } = height;
    public int ZIndex { get; set; } = zindex ?? 0;
    public string? BackgroundColor { get; set; } = backgroundColor;
}


