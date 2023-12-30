namespace SoftTouch.UI.Flexbox;

public class TextElement(
    string text = "",
    ViewNumber? fontSize = null,
    string color = "#000",
    string? id = null
    ) : FlexElement(id: id)
{
    public string Text { get; set; } = text;
    public ViewNumber FontSize { get; set; } = fontSize ?? 12;
    public string Color { get; set; } = color;

    public override string ToString()
    {
        return $"[color : {Color}, size : {FontSize}] \"{Text}\"";
    }
}


