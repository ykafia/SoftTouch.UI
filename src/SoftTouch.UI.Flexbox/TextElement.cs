using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace SoftTouch.UI.Flexbox;

public class TextStyle : FlexStyle
{
    public string? Text 
    { 
        get => styleData.GetProperty<string>(nameof(Text)); 
        set 
        { 
            styleData.SetProperty(nameof(Text), value); 
            UpdateSize(); 
        } 
    }
    public string? FontFamily
    { 
        get => styleData.GetProperty<string>(nameof(FontFamily)); 
        set 
        { 
            styleData.SetProperty(nameof(FontFamily), value); 
            UpdateSize(); 
        } 
    }
    public ViewNumber? FontSize
    { 
        get => styleData.GetProperty<ViewNumber>(nameof(FontSize)); 
        set 
        { 
            styleData.SetProperty(nameof(FontSize), value); 
            UpdateSize(); 
        } 
    }
    public string? Color
    { 
        get => styleData.GetProperty<string>(nameof(Color)); 
        set => styleData.SetProperty(nameof(Color), value);
    }


    public TextStyle(
        string text = "",
        string fontFamily = "Fira Sans",
        ViewNumber? fontSize = null,
        string color = "#000",
        string? id = null
    ) : base(id: id)
    {
        Text = text;
        FontFamily = fontFamily;
        FontSize = fontSize ?? 12;
        Color = color;
        UpdateSize();
    }

    public void UpdateSize()
    {
        var size = TextMeasurer.MeasureSize(Text ?? "", new(SystemFonts.CreateFont(FontFamily ?? "Roboto", (float)(FontSize ?? 16))));
        Width = size.Width;
        Height = size.Height;
    }

    public override string ToString()
    {
        return $"[color : {Color}, size : {FontSize}] \"{Text}\"";
    }
}


