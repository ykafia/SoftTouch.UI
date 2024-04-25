using System.Data.SqlTypes;
using Microsoft.Extensions.ObjectPool;

namespace SoftTouch.UI.Flexbox;


public sealed class StyleData
{
    internal static readonly ObjectPool<StyleData> pool = ObjectPool.Create<StyleData>();

    readonly Dictionary<string, string> properties = [];
    internal T? GetProperty<T>(string propertyName) where T : IParsable<T>
    {
        if(properties.TryGetValue(propertyName, out var value))
            return T.Parse(value, null);
        else return default;
    }
    internal void SetProperty<T>(string propertyName, T? value)
        => properties[propertyName] = value?.ToString() ?? "";
    internal T? GetEnumProperty<T>(string propertyName) where T : struct, Enum
    {
        if(properties.TryGetValue(propertyName, out var value)) 
            return Enum.Parse<T>(value);
        else return default;
    }
    internal void SetProperty<T>(string propertyName, T? value) where T : struct, Enum
        => properties[propertyName] = value?.ToString() ?? "";
}
