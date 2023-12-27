using SkiaSharp;
using SoftTouch.UI.Example;
using SoftTouch.UI.Flexbox;

var text = new TextView()
{
    TextStyle = new("Hello world!", 24, "#FFF"),
    BackgroundColor = "#000",
    Height = 24,
    Width = 128,
    X = 100,
    Y = 100 + 22
};
var box = new BoxView()
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
};
var tree = new FlexTree();
tree.AddChild(null, box);
tree.AddChild(box, text);
var skiar = new SkiaRenderer(tree);
skiar.Render();
skiar.SavePng();