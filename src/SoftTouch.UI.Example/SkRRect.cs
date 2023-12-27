using System.Runtime.CompilerServices;
using SkiaSharp;

namespace SoftTouch.UI.Example;


public struct SKRRect(SKRect r)
{
    SKRect rect = r;

    public SKPoint Location { readonly get => rect.Location; set => rect.Location = value; }
    public SKSize Size { readonly get => rect.Size; set => rect.Size = value; }
    public float Bottom { readonly get => rect.Bottom; set => rect.Bottom = value; }
    public float Top { readonly get => rect.Top; set => rect.Top = value; }
    public float Left { readonly get => rect.Left; set => rect.Left = value; }
    public float Right { readonly get => rect.Right; set => rect.Right = value; }


    public float TopRightRadius { get; set; }
    public float TopLeftRadius { get; set; }
    public float BottomRightRadius { get; set; }
    public float BottomLeftRadius { get; set; }

    public static implicit operator SKRRect(SKRect rect) => new(rect);
}



public static class SkiaCanvasExtensions
{
    public static void DrawRoundedRect(this SKCanvas canvas, SKRRect rect, SKPaint paint)
    {
        using var path = new SKPath();
        // Move to point just below top corner
        path.MoveTo(rect.Left, rect.Top + rect.TopLeftRadius);
        if(rect.TopLeftRadius > 0)
            path.ArcTo(
                new SKRect(
                    rect.Left, 
                    rect.Top,
                    rect.Left + rect.TopLeftRadius, 
                    rect.Top + rect.TopLeftRadius
                ),
                180,
                90,
                true
            );
        else
            path.LineTo(rect.Left,rect.Top);
        if(rect.TopRightRadius > 0)
            path.ArcTo(
                new SKRect(
                    rect.Right - rect.TopRightRadius, 
                    rect.Top,
                    rect.Right, 
                    rect.Top + rect.TopRightRadius
                ),
                270,
                90,
                false
            );
        else
            path.LineTo(rect.Right,rect.Top);
        if(rect.BottomRightRadius > 0)
            path.ArcTo(
                new SKRect(
                    rect.Right - rect.BottomRightRadius, 
                    rect.Bottom - rect.BottomRightRadius,
                    rect.Right, 
                    rect.Bottom
                ),
                0,
                90,
                false
            );
        else
            path.LineTo(rect.Right,rect.Bottom);
        
        if(rect.BottomLeftRadius > 0)
            path.ArcTo(
                new SKRect(
                    rect.Left, 
                    rect.Bottom - rect.BottomLeftRadius,
                    rect.Left + rect.BottomLeftRadius, 
                    rect.Bottom
                ),
                90,
                90,
                false
            );
        else
            path.LineTo(rect.Left, rect.Bottom + rect.BottomLeftRadius);
        canvas.DrawPath(path, paint);
    }
}
