using SkiaSharp;

var imageInfo = new SKImageInfo(
    width: 1024,
    height: 1024,
    colorType: SKColorType.Rgba8888,
    alphaType: SKAlphaType.Premul);

var surface = SKSurface.Create(imageInfo);

var canvas = surface.Canvas;


canvas.DrawCircle(new(512,512),512, new(){Color = SKColor.Parse("#FF0000")});

using var image = surface.Snapshot();
using var data = image.Encode(SKEncodedImageFormat.Png, 80);
using var fs = File.OpenWrite("./result.png");
data.SaveTo(fs);

