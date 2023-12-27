using System.Runtime.CompilerServices;
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

    public void Render<T>(Tree<T> node)
        where T : FixedView
    {
        var viewList = new List<FixedView>();
        Canvas.Clear(SKColor.Parse("#FFF"));
        var queue = new FlexQueue<Tree>();
        queue.Enqueue(node);
        while(queue.Count > 0)
        {
            var n = queue.DequeueFront() ?? throw new NotImplementedException();

            FixedView fixedv = n switch
            {
                Tree<BoxView> b => b.Value ?? throw new NotImplementedException(),
                Tree<TextView> t => t.Value ?? throw new NotImplementedException(),
                _ => throw new NotImplementedException()
            } ;

            viewList.Add(fixedv);
            var p = n.LastChild;

            while(p != null)
            {
                if(p is Tree<BoxView> tbv && tbv.Value is BoxView bv && bv.ViewStyle.DisplayFlex)
                {
                    p = p.Prev as Tree<BoxView>;
                    continue;
                }
                else if(p is Tree<TextView> ttv && ttv.Value is TextView tv)
                {
                    p = p.Prev as Tree<TextView>;
                    continue;
                }
                Tree v = p switch 
                {
                    Tree<BoxView> a => a,
                    Tree<TextView> b => b,
                    _ => throw new NotImplementedException()
                };
                queue.Enqueue(v);
                p = p.Prev;
            }
        }
        viewList.Sort(static (a,b) => a.ZIndex - b.ZIndex);
        foreach(var view in viewList)
        {
            
            if(view is BoxView bv)
            {
                var rect = new SKRect(bv.X, bv.Y, bv.X + bv.Width, bv.Y + bv.Height);
                var paint = new SKPaint()
                {
                    Color = SKColor.Parse(bv.BackgroundColor)
                };
                Canvas.DrawRect(rect, paint);
            }
            else if(view is TextView tv)
            {
                Canvas.DrawText(tv.Text, new(tv.X,tv.Y),new(){
                    Color = SKColor.Parse(tv.TextStyle.Color),
                    TextSize = tv.TextStyle.FontSize,

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
