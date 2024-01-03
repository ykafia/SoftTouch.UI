using SixLabors.Fonts;
using SkiaSharp;
using SoftTouch.UI.Example;
using SoftTouch.UI.Flexbox;


var view = new BoxView(
    new BoxElement(
        id : "parent",
        backgroundColor : "#111",
        grow : 1,
        flexDirection : FlexDirection.Row,
        x: 0,
        y: 0,
        width: 1024,
        height: 1024
    ),
    [
        new BoxElement(id:  "1",width: 100, height: 100, backgroundColor: "#FAAA", alignSelf: FlexAlignment.Center),
        new BoxElement(id:  "2",width: 100, height: 100, backgroundColor: "#FCCC", alignSelf: FlexAlignment.FlexEnd),
        new BoxElement(id:  "3",width: 100, height: 100, backgroundColor: "#FDDD", alignSelf: FlexAlignment.Center),
        new BoxElement(id:  "4",width: 100, height: 100, backgroundColor: "#FFFF", alignSelf: FlexAlignment.FlexStart),
        new BoxElement(id:  "5",width: 100, height: 100, backgroundColor: "#FFAF", alignSelf: FlexAlignment.Center),
        new BoxElement(id:  "6",width: 100, height: 100, backgroundColor: "#FFBF", alignSelf: FlexAlignment.Stretch),
        // new BoxElement(id:  "7",width: 100, height: 100, backgroundColor: "#FCCF"),
        // new BoxElement(id:  "8",width: 100, height: 100, backgroundColor: "#FBCF"),
        // new BoxElement(id:  "9",width: 100, height: 100, backgroundColor: "#FACF"),
        // new BoxElement(id: "10",width: 100, height: 100, backgroundColor: "#FA3F"),
        // new BoxElement(id: "11",width: 100, height: 100, backgroundColor: "#F43F"),
        // new BoxElement(id: "12",width: 100, height: 100, backgroundColor: "#F93F"),
        // new BoxElement(id: "13",width: 100, height: 100, backgroundColor: "#FFDD")
    ]
);

var renderer = new SkiaRenderer(view);
renderer.Render();
renderer.SavePng();