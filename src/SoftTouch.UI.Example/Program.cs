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
        height: 1024,
        justifyContent: JustifyContent.SpaceAround
    ),
    [
        new BoxElement(id: "1",width: 100, height: 100, backgroundColor: "#AAA"),
        new BoxElement(id: "2",width: 100, height: 100, backgroundColor: "#CCC"),
        new BoxElement(id: "3",width: 100, height: 100, backgroundColor: "#DDD"),
        new BoxElement(id: "4",width: 100, height: 100, backgroundColor: "#FFF")
    ]
);

var renderer = new SkiaRenderer(view);
renderer.Render();
renderer.SavePng();