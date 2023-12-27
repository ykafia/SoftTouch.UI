using SkiaSharp;
using SoftTouch.UI.Example;
using SoftTouch.UI.Flexbox;

var text = new Tree<TextView>()
{
    Value = new()
    {
        TextStyle =  new("Hello world!", 24, "#000"),
        BackgroundColor = "#000",
        Height = 24,
        Width = 128,
        X = 100,
        Y = 200
    }
};
var box = new Tree<BoxView>()
{
    Value = new()
    {
        BackgroundColor = "#0AA",
        X = 100,
        Y = 100,
        Height = 24,
        Width = 128,
        ViewStyle = new()
        {
            DisplayFlex = true
        }
    }
};
box.AddChild(text);
var skiar = new SkiaRenderer(box);
skiar.Render(box);
skiar.SavePng();