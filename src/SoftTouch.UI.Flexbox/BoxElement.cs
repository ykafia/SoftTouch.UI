using System.Text.RegularExpressions;

namespace SoftTouch.UI.Flexbox;

public class BoxElement : FlexStyle
{
    public ViewNumber? Padding
    {
        get => ViewNumber.MaxMagnitude(ViewNumber.MaxMagnitude(ViewNumber.MaxMagnitude(PaddingLeft ?? 0, PaddingRight ?? 0), PaddingBottom ?? 0), PaddingTop ?? 0);
        set
        {
            PaddingTop = value;
            PaddingBottom = value;
            PaddingLeft = value;
            PaddingRight = value;
        }
    }

    public ViewNumber? PaddingHorizontal
    {
        get => ViewNumber.MaxMagnitude(PaddingBottom ?? 0, PaddingTop ?? 0);
        set
        {
            PaddingTop = value;
            PaddingBottom = value;
        }
    }
    public ViewNumber? PaddingVertical
    {
        get => ViewNumber.MaxMagnitude(PaddingLeft ?? 0, PaddingRight ?? 0);
        set
        {
            PaddingLeft = value;
            PaddingRight = value;
        }
    }
    public ViewNumber? Margin
    {
        get => ViewNumber.MaxMagnitude(ViewNumber.MaxMagnitude(ViewNumber.MaxMagnitude(MarginLeft ?? 0, MarginRight ?? 0), MarginBottom ?? 0), MarginTop ?? 0);
        set
        {
            MarginTop = value;
            MarginBottom = value;
            MarginLeft = value;
            MarginRight = value;
        }
    }
    public ViewNumber? MarginHorizontal
    {
        get => ViewNumber.MaxMagnitude(MarginBottom ?? 0, MarginTop ?? 0);
        set
        {
            MarginTop = value;
            MarginBottom = value;
        }
    }
    public ViewNumber? MarginVertical
    {
        get => ViewNumber.MaxMagnitude(MarginLeft ?? 0, MarginRight ?? 0);
        set
        {
            MarginLeft = value;
            MarginRight = value;
        }
    }
    public FlexDirection? FlexDirection
    {
        get => styleData.GetEnumProperty<FlexDirection>(nameof(FlexDirection));
        set => styleData.SetProperty(nameof(FlexDirection), value);
    }
    public JustifyContent? JustifyContent
    {
        get => styleData.GetEnumProperty<JustifyContent>(nameof(JustifyContent));
        set => styleData.SetProperty(nameof(JustifyContent), value);
    }
    public FlexWrap? FlexWrap
    {
        get => styleData.GetEnumProperty<FlexWrap>(nameof(FlexWrap));
        set => styleData.SetProperty(nameof(FlexWrap), value);
    }
    public FlexContentAlignment? AlignContent
    {
        get => styleData.GetEnumProperty<FlexContentAlignment>(nameof(AlignContent));
        set => styleData.SetProperty(nameof(AlignContent), value);
    }
    public FlexAlignment? AlignItems
    {
        get => styleData.GetEnumProperty<FlexAlignment>(nameof(AlignItems));
        set => styleData.SetProperty(nameof(AlignItems), value);
    }
    public FlexAlignment? AlignSelf
    {
        get => styleData.GetEnumProperty<FlexAlignment>(nameof(AlignSelf));
        set => styleData.SetProperty(nameof(AlignSelf), value);
    }
    public ViewNumber? Grow
    {
        get => styleData.GetProperty<ViewNumber>(nameof(Grow));
        set => styleData.SetProperty(nameof(Grow), value);
    }
    public ViewNumber? Shrink
    {
        get => styleData.GetProperty<ViewNumber>(nameof(Shrink));
        set => styleData.SetProperty(nameof(Shrink), value);
    }
    public FlexPosition? Position
    {
        get => styleData.GetEnumProperty<FlexPosition>(nameof(Position));
        set => styleData.SetProperty(nameof(Position), value);
    }
    public ViewNumber? Gap
    {
        get => styleData.GetProperty<ViewNumber>(nameof(Gap));
        set => styleData.SetProperty(nameof(Gap), value);
    }
    public string? Display
    {
        get => styleData.GetProperty<string>(nameof(Display));
        set => styleData.SetProperty(nameof(Display), value);
    }
    public ViewNumber? Top
    {
        get => styleData.GetProperty<ViewNumber>(nameof(Top));
        set => styleData.SetProperty(nameof(Top), value);
    }
    public ViewNumber? Left
    {
        get => styleData.GetProperty<ViewNumber>(nameof(Left));
        set => styleData.SetProperty(nameof(Left), value);
    }
    public ViewNumber? Right
    {
        get => styleData.GetProperty<ViewNumber>(nameof(Right));
        set => styleData.SetProperty(nameof(Right), value);
    }
    public ViewNumber? Bottom
    {
        get => styleData.GetProperty<ViewNumber>(nameof(Bottom));
        set => styleData.SetProperty(nameof(Bottom), value);
    }

    public ViewNumber? PaddingLeft
    {
        get => styleData.GetProperty<ViewNumber>(nameof(PaddingLeft));
        set => styleData.SetProperty(nameof(PaddingLeft), value);
    }
    public ViewNumber? PaddingRight
    {
        get => styleData.GetProperty<ViewNumber>(nameof(PaddingRight));
        set => styleData.SetProperty(nameof(PaddingRight), value);
    }
    public ViewNumber? PaddingTop
    {
        get => styleData.GetProperty<ViewNumber>(nameof(PaddingTop));
        set => styleData.SetProperty(nameof(PaddingTop), value);
    }
    public ViewNumber? PaddingBottom
    {
        get => styleData.GetProperty<ViewNumber>(nameof(PaddingBottom));
        set => styleData.SetProperty(nameof(PaddingBottom), value);
    }

    public ViewNumber? MarginLeft
    {
        get => styleData.GetProperty<ViewNumber>(nameof(MarginLeft));
        set => styleData.SetProperty(nameof(MarginLeft), value);
    }
    public ViewNumber? MarginRight
    {
        get => styleData.GetProperty<ViewNumber>(nameof(MarginRight));
        set => styleData.SetProperty(nameof(MarginRight), value);
    }
    public ViewNumber? MarginTop
    {
        get => styleData.GetProperty<ViewNumber>(nameof(MarginTop));
        set => styleData.SetProperty(nameof(MarginTop), value);
    }
    public ViewNumber? MarginBottom
    {
        get => styleData.GetProperty<ViewNumber>(nameof(MarginBottom));
        set => styleData.SetProperty(nameof(MarginBottom), value);
    }


    public BoxElement(
        string? id = null,
        ViewNumber? x = null,
        ViewNumber? y = null,
        ViewNumber? width = null,
        ViewNumber? height = null,
        int? zindex = null,
        string? backgroundColor = null,
        FlexDirection? flexDirection = null,
        JustifyContent? justifyContent = null,
        FlexWrap? flexWrap = null,
        FlexContentAlignment? alignContent = null,
        FlexAlignment? alignItems = null,
        FlexAlignment? alignSelf = null,
        ViewNumber? grow = null,
        ViewNumber? shrink = null,
        FlexPosition? position = null,
        ViewNumber? gap = null,
        string? display = null,
        ViewNumber? left = null,
        ViewNumber? right = null,
        ViewNumber? top = null,
        ViewNumber? bottom = null,
        ViewNumber? paddingLeft = null,
        ViewNumber? paddingRight = null,
        ViewNumber? paddingTop = null,
        ViewNumber? paddingBottom = null,
        ViewNumber? marginLeft = null,
        ViewNumber? marginRight = null,
        ViewNumber? marginTop = null,
        ViewNumber? marginBottom = null,
        ViewNumber? margin = null,
        ViewNumber? padding = null
    ) : base(id, x, y, width, height, zindex, backgroundColor)
    {
        FlexDirection = flexDirection ?? Flexbox.FlexDirection.Row;
        JustifyContent = justifyContent ?? Flexbox.JustifyContent.FlexStart;
        FlexWrap = flexWrap ?? Flexbox.FlexWrap.NoWrap;
        AlignContent = alignContent ?? FlexContentAlignment.FlexStart;
        AlignItems = alignItems;
        AlignSelf = alignSelf;
        Grow = grow ?? 0;
        Shrink = shrink ?? 1;
        Position = position ?? FlexPosition.Relative;
        Gap = gap;
        Display = display;
        Left = left;
        Right = right;
        Top = top;
        Bottom = bottom;
        PaddingLeft = paddingLeft;
        PaddingRight = paddingRight;
        PaddingTop = paddingTop;
        PaddingBottom = paddingBottom;
        MarginLeft = marginLeft;
        MarginRight = marginRight;
        MarginTop = marginTop;
        MarginBottom = marginBottom;
        if (margin != null)
            Margin = margin;
        if (padding != null)
            Padding = padding;
    }
    public override string ToString()
    {
        return $"[color : {BackgroundColor}] {Id ?? "Box"}";
    }
}


