# SoftTouch UI

An attempt at making a backend agnostic flexbox layout engine from scratch.

```csharp
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
            margin: 25
        ),
        new BoxElement(
            id : "child1",
            backgroundColor : "#FF0",
            width : 200,
            height : 100,
            margin: 25
        )
    ]
);
var renderer = new SkiaRenderer(new(view));
renderer.Render();
renderer.SavePng();
```