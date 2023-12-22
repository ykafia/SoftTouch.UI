using SkiaSharp;
using SoftTouch.UI.Example;
using SoftTouch.UI.Flexbox;

var tree = new Tree<TextView>()
{
    Value = new()
    {
        TextStyle =  new("Hello world!", 12, "#000"),
        BackgroundColor = "#FFF",
        Height = 24,
        Width = 128,
    }
};
var skiar = new SkiaRenderer(tree);
skiar.Render();
skiar.SavePng();