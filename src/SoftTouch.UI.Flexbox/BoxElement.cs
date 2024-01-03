using System.Text.RegularExpressions;

namespace SoftTouch.UI.Flexbox;

public class BoxElement : FlexElement
{
    public FlexDirection? FlexDirection { get; set; }
    public JustifyContent? JustifyContent { get; set; }
    public FlexWrap? FlexWrap { get; set; }
    public FlexContentAlignment? AlignContent { get; set; }
    public FlexAlignment? AlignItems { get; set; }
    public FlexAlignment? AlignSelf { get; set; }
    public ViewNumber? Grow { get; set; }
    public ViewNumber? Shrink { get; set; }
    public FlexPosition? Position { get; set; }
    public ViewNumber? Gap { get; set; }
    public string? Display { get; set; }
    public ViewNumber? Top { get; set; }
    public ViewNumber? Left { get; set; }
    public ViewNumber? Right { get; set; }
    public ViewNumber? Bottom { get; set; }
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
    public ViewNumber? PaddingLeft { get; set; }
    public ViewNumber? PaddingRight { get; set; }
    public ViewNumber? PaddingTop { get; set; }
    public ViewNumber? PaddingBottom { get; set; }
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
    public ViewNumber? MarginLeft { get; set; }
    public ViewNumber? MarginRight { get; set; }
    public ViewNumber? MarginTop { get; set; }
    public ViewNumber? MarginBottom { get; set; }


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


