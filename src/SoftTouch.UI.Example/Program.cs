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
        justifyContent : JustifyContent.Center
    ),
    [
        new BoxElement(
            id : "child0",
            backgroundColor : "#330",
            width : 200,
            height : 100,
            alignSelf : FlexAlignment.Stretch,
            marginTop : 10,
            marginLeft : 10

        ),
        new BoxElement(
            id : "child1",
            backgroundColor : "#FF0",
            width : 200,
            height : 100,
            marginTop : 10,
            marginLeft : 10
        )
    ]
);
var renderer = new SkiaRenderer(new(view));
renderer.Render();
renderer.SavePng();