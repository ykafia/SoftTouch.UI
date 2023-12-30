using System.Runtime.CompilerServices;
using SkiaSharp;
using SoftTouch.UI.Flexbox;

namespace SoftTouch.UI.Example;

public class SkiaRenderer : IFlexRenderer
{
    public SKImageInfo ImageInfo { get; }
    public SKSurface Surface { get; }
    public SKCanvas Canvas => Surface.Canvas;
    public FlexTree RenderTree { get; set; }

    public SkiaRenderer(FlexTree tree, int width = 1024, int height = 1024)
    {
        RenderTree = tree;
        ImageInfo = new(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
        Surface = SKSurface.Create(ImageInfo);
    }

    public void Render()
    {
        Canvas.Clear(SKColor.Parse("#FFF"));

        
        foreach(var view in RenderTree)
        {
            if(view is BoxView bv)
            {
                var rect = new SKRect((float?)bv.X ?? 0, (float?)bv.Y ?? 0, (float?)(bv.X + bv.Width) ?? 0, (float?)(bv.Y + bv.Height) ?? 0);
                var paint = new SKPaint()
                {
                    Color = SKColor.Parse(bv.BackgroundColor)
                };
                Canvas.DrawRect(rect, paint);
            }
            else if(view is TextView tv)
            {
                Canvas.DrawText(tv.Text, new((float?)tv.X ?? 0,(float?)tv.Y ?? 0),new(){
                    Color = SKColor.Parse(tv.Color),
                    TextSize = (float?)tv.FontSize ?? 0,
                });
            }
        }
    }

    public void SavePng()
    {
        using var image = Surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        using var fs = File.OpenWrite("./result.png");
        data.SaveTo(fs);
    }
}
