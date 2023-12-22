using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace SoftTouch.UI.Flexbox;


public enum ViewNumberKind
{
    Number,
    Percentage,
    Degree
}

public record struct ViewNumber(double Value, ViewNumberKind Kind)
{
    public static implicit operator ViewNumber(string s)
    {
        return s switch 
        {
            string v when s.EndsWith('%') => new(double.Parse(v.AsSpan()[..^1]),ViewNumberKind.Percentage),
            string v when s.EndsWith("deg") => new(double.Parse(v.AsSpan()[..^1]), ViewNumberKind.Degree),
            string v => new(double.Parse(v),ViewNumberKind.Number)
        };
    }
}