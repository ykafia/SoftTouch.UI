# SoftTouch UI

An attempt at making a backend agnostic flexbox layout engine from scratch.


## Features 

* [x] Relative position
* [x] Absolute position
* [x] Flex grow
* [x] Flex shrink
* [x] Align items
    * [x] FlexStart
    * [x] FlexEnd
    * [x] FlexCenter
    * [x] Stretch
* [x] Align self
    * [x] FlexStart
    * [x] FlexEnd
    * [x] FlexCenter
    * [x] Stretch
* [ ] Wrapping
* [ ] Align content
    * [ ] FlexStart
    * [ ] FlexEnd
    * [ ] FlexCenter
    * [ ] Stretch
    * [ ] Space between
    * [ ] Space around
* [ ] Caching states

## API



```csharp

// Creating a box view containing a box element and a list of box view implicitely created from a box elements
var view = new BoxView(
    new BoxElement(
        id : "parent",
        backgroundColor : "#111",
        grow : 1,
        flexDirection : FlexDirection.Row,
        x: 0,
        y: 0,
        width: 1024 * 2,
        height: 1024 * 2
    ),
    [
        new BoxElement(id:  "1",width: 100, height: 100, backgroundColor: "#FAAA", grow: 1),
        new BoxElement(id:  "2",width: 100, height: 100, backgroundColor: "#FCCC"),
        new BoxElement(id:  "3",width: 100, height: 100, backgroundColor: "#FDDD"),
        new BoxElement(id:  "4",width: 100, height: 100, backgroundColor: "#FFFF", alignSelf: FlexAlignment.Center),
        new BoxElement(id:  "5",width: 100, height: 100, backgroundColor: "#FFAF", alignSelf: FlexAlignment.Center, left: 10),
        new BoxElement(id:  "6",width: 100, height: 100, backgroundColor: "#FFBF", alignSelf: FlexAlignment.Center),
        new BoxElement(id:  "7",width: 100, height: 100, backgroundColor: "#FCCF"),
        new BoxElement(id:  "8",width: 100, height: 100, backgroundColor: "#FBCF"),
        new BoxElement(id:  "9",width: 100, height: 100, backgroundColor: "#FACF", shrink: 0),
        new BoxElement(id: "10",width: 100, height: 100, backgroundColor: "#FA3F"),
        new BoxElement(id: "11",width: 100, height: 100, backgroundColor: "#F43F"),
        new BoxElement(id: "12",width: 100, height: 100, backgroundColor: "#F93F"),
        new BoxElement(id: "13",width: 100, height: 100, backgroundColor: "#FFDD", grow: 1, alignSelf: FlexAlignment.Stretch)
    ]
);

// You should then create a tree from this and call the update function

FlexTree tree = new(view);
tree.Update();

// Or you can create your own renderer (one is present in the example project)

var renderer = new SkiaRenderer(view);
renderer.Render();
renderer.SavePng();
```