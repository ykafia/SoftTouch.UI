using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace SoftTouch.UI.Flexbox;

public class TextElement : FlexElement
{
    string _text;
    string _fontFamily;
    ViewNumber _size;
    public string Text { get => _text; set { _text = value; UpdateSize(); } }
    public string FontFamily { get => _fontFamily; set {_fontFamily = value; UpdateSize();} }
    public ViewNumber FontSize { get => _size; set { _size = value; UpdateSize(); } }
    public string Color { get; set; }


    public TextElement(
        string text = "",
        string fontFamily = "Fira Sans",
        ViewNumber? fontSize = null,
        string color = "#000",
        string? id = null
    ) : base(id: id)
    {
        _text = text;
        _fontFamily = fontFamily;
        _size = fontSize ?? 12;
        Color = color;
        UpdateSize();
    }

    public void UpdateSize()
    {
        var size = TextMeasurer.MeasureSize(Text, new(SystemFonts.CreateFont(FontFamily, (float)FontSize)));
        Width = size.Width;
        Height = size.Height;
    }

    public override string ToString()
    {
        return $"[color : {Color}, size : {FontSize}] \"{Text}\"";
    }


}


