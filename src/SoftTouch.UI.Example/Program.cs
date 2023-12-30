using SkiaSharp;
using SoftTouch.UI.Example;
using SoftTouch.UI.Flexbox;


var view = new BoxView(
    new(){
        Width = 300,
        Height = 200,
        BackgroundColor = "#0FF",
        Flex = 1,
        FlexDirection = FlexDirection.Row,
        Position = ViewPosition.None
    },
    [
        new BoxElement()
        {
            BackgroundColor = "#330"
        },
        new BoxElement()
        {
            BackgroundColor = "#FF0"
        }
    ]
);
var renderer = new SkiaRenderer(new(view));
renderer.Render();
renderer.SavePng();