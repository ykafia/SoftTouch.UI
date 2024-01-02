using SixLabors.Fonts;
using SkiaSharp;
using SoftTouch.UI.Example;
using SoftTouch.UI.Flexbox;


var view = new BoxView(
    new BoxElement(
        id : "parent",
        backgroundColor : "#0FF",
        grow : 1,
        flexDirection : FlexDirection.Row,
        x: 0,
        y: 0,
        width: 1024,
        height: 1024
    ),
    [
        (
            new BoxElement(
                id : "child0",
                backgroundColor : "#330",
                position: FlexPosition.Absolute,
                left: 10,
                right: 100,
                top: 10,
                bottom: 40

            ),
            []
        )
    ]
);

var renderer = new SkiaRenderer(view);
renderer.Render();
renderer.SavePng();