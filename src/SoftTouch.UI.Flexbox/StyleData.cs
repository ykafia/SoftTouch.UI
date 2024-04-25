using Microsoft.Extensions.ObjectPool;

namespace SoftTouch.UI.Flexbox;


public abstract class StyleData
{
    protected Dictionary<string, string> properties = [];
    internal T? GetProperty<T>(string propertyName) where T : IParsable<T>
        => T.Parse(properties[propertyName], null);
    internal void SetProperty<T>(string propertyName) where T : IParsable<T>
        => T.Parse(properties[propertyName], null);
    internal T? GetEnumProperty<T>(string propertyName) where T : struct, Enum
        => Enum.Parse<T>(properties[propertyName]);
    internal void SetEnumProperty<T>(string propertyName, T value) where T : struct, Enum
        => properties[propertyName] = value.ToString();
}

public enum Machin
{
    Machin1,
    Machin2
}
internal class CssClass : StyleData
{
    static readonly ObjectPool<CssClass> pool = ObjectPool.Create<CssClass>();
    public Machin? MachinValue
    {
        get => GetEnumProperty<Machin>(nameof(MachinValue));
        set => SetEnumProperty(nameof(MachinValue), value ?? throw new Exception());
    }

    public CssClass()
    {

    }

    ~CssClass()
    {

    }
}