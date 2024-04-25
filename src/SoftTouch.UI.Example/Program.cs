using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
        new BoxElement(id:  "1",width: 100, height: "100%", backgroundColor: "#FAAA", grow: 1),
        new BoxElement(id:  "2",width: 100, height: "100%", backgroundColor: "#FCCC"),
        
    ]
);

var renderer = new SkiaRenderer(view);
renderer.Render();
renderer.SavePng();