using SkiaSharp;
using SoftTouch.UI.Flexbox;

namespace SoftTouch.UI.Example;

public class SkiaRenderer : IFlexRenderer
{
    public SKImageInfo ImageInfo { get; }
    public SKSurface Surface { get; }
    public SKCanvas Canvas => Surface.Canvas;
    public Tree RenderTree { get; set; }

    public SkiaRenderer(Tree tree, int width = 1024, int height = 1024)
    {
        RenderTree = tree;
        ImageInfo = new(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
        Surface = SKSurface.Create(ImageInfo);
    }

    public void Render()
    {
        var viewList = new List<FixedView>();
        Canvas.Clear(SKColor.Parse("#FFF"));
        Canvas.DrawCircle(new(512, 512), 512, new() { Color = SKColor.Parse("#FF0000") });
    }

    public void SavePng()
    {
        using var image = Surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        using var fs = File.OpenWrite("./result.png");
        data.SaveTo(fs);
    }
}
