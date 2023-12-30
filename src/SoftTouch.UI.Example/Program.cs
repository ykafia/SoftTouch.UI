using SkiaSharp;
using SoftTouch.UI.Example;
using SoftTouch.UI.Flexbox;


var view = new BoxView(
    new BoxElement(
        id : "parent",
        backgroundColor : "#0FF",
        flex : 1,
        flexDirection : FlexDirection.Row,
        position: ViewPosition.Relative,
        padding: 10
    ),
    [
        new BoxElement(
            id : "child0",
            backgroundColor : "#330",
            width : 200,
            height : 100,
            alignSelf : FlexAlignment.Stretch
        ),
        new BoxElement(
            id : "child1",
            backgroundColor : "#FF0",
            width : 200,
            height : 100
        )
    ]
);
var renderer = new SkiaRenderer(new(view));
renderer.Render();
renderer.SavePng();