namespace SoftTouch.UI.Flexbox;

public class FlexStyle
{
    protected readonly StyleData styleData = StyleData.pool.Get();


    public string? Id
    {
        get => styleData.GetProperty<string>(nameof(Id));
        set => styleData.SetProperty(nameof(Id), value);
    }
    public ViewNumber? X
    {
        get => styleData.GetProperty<ViewNumber>(nameof(X));
        set => styleData.SetProperty(nameof(X), value);
    }
    public ViewNumber? Y
    {
        get => styleData.GetProperty<ViewNumber>(nameof(Y));
        set => styleData.SetProperty(nameof(Y), value);
    }
    public ViewNumber? Width
    {
        get => styleData.GetProperty<ViewNumber>(nameof(Width));
        set => styleData.SetProperty(nameof(Width), value);
    }
    public ViewNumber? Height
    {
        get => styleData.GetProperty<ViewNumber>(nameof(Height));
        set => styleData.SetProperty(nameof(Height), value);
    }
    public int ZIndex
    {
        get => styleData.GetProperty<int>(nameof(ZIndex));
        set => styleData.SetProperty(nameof(ZIndex), value);
    }
    public string? BackgroundColor
    {
        get => styleData.GetProperty<string>(nameof(BackgroundColor));
        set => styleData.SetProperty(nameof(BackgroundColor), value);
    }


    public FlexStyle(
        string? id = null,
        ViewNumber? x = null,
        ViewNumber? y = null,
        ViewNumber? width = null,
        ViewNumber? height = null,
        int? zindex = null,
        string? backgroundColor = null
    )
    {
        Id = id;
        X = x ?? 0;
        Y = y ?? 0;
        Width = width;
        Height = height;
        ZIndex = zindex ?? 0;
        BackgroundColor = backgroundColor;
    }


    ~FlexStyle()
    {
        StyleData.pool.Return(styleData);
    }
}