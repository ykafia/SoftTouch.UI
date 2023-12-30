using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace SoftTouch.UI.Flexbox;


public enum ViewNumberKind
{
    Number,
    Percentage,
    Degree
}

public record struct ViewNumber(double Value, ViewNumberKind Kind) : INumber<ViewNumber>
{

    public readonly float Percentage => Kind == ViewNumberKind.Percentage ? (float)(Value / 100) : throw new Exception($"Cannot get percentage of kind {Kind}");


    public static ViewNumber One => throw new NotImplementedException();

    public static int Radix => throw new NotImplementedException();

    public static ViewNumber Zero => throw new NotImplementedException();

    public static ViewNumber AdditiveIdentity => throw new NotImplementedException();

    public static ViewNumber MultiplicativeIdentity => throw new NotImplementedException();

    static ViewNumber INumberBase<ViewNumber>.One => throw new NotImplementedException();

    static int INumberBase<ViewNumber>.Radix => throw new NotImplementedException();

    static ViewNumber INumberBase<ViewNumber>.Zero => throw new NotImplementedException();

    static ViewNumber IAdditiveIdentity<ViewNumber, ViewNumber>.AdditiveIdentity => throw new NotImplementedException();

    static ViewNumber IMultiplicativeIdentity<ViewNumber, ViewNumber>.MultiplicativeIdentity => throw new NotImplementedException();

    public static ViewNumber Abs(ViewNumber value) => new(Math.Abs(value.Value), value.Kind);

    public static bool IsCanonical(ViewNumber value) => false;
    public static bool IsComplexNumber(ViewNumber value) => false;

    public static bool IsEvenInteger(ViewNumber value) => double.IsEvenInteger(value.Value);

    public static bool IsFinite(ViewNumber value) => double.IsFinite(value.Value);

    public static bool IsImaginaryNumber(ViewNumber value) => false;
    public static bool IsInfinity(ViewNumber value) => double.IsInfinity(value.Value);

    public static bool IsInteger(ViewNumber value) => double.IsInteger(value.Value);

    public static bool IsNaN(ViewNumber value) => double.IsNaN(value.Value);

    public static bool IsNegative(ViewNumber value) => double.IsNegative(value.Value);

    public static bool IsNegativeInfinity(ViewNumber value) => double.IsNegativeInfinity(value.Value);

    public static bool IsNormal(ViewNumber value) => double.IsNormal(value.Value);

    public static bool IsOddInteger(ViewNumber value) => double.IsOddInteger(value.Value);

    public static bool IsPositive(ViewNumber value) => double.IsPositive(value.Value);

    public static bool IsPositiveInfinity(ViewNumber value) => double.IsPositiveInfinity(value.Value);

    public static bool IsRealNumber(ViewNumber value) => double.IsRealNumber(value.Value);

    public static bool IsSubnormal(ViewNumber value) => double.IsSubnormal(value.Value);

    public static bool IsZero(ViewNumber value) => value.Value == 0;

    public static ViewNumber MaxMagnitude(ViewNumber x, ViewNumber y) => double.MaxMagnitude(x.Value, y.Value);

    public static ViewNumber MaxMagnitudeNumber(ViewNumber x, ViewNumber y) => double.MaxMagnitudeNumber(x.Value, y.Value);

    public static ViewNumber MinMagnitude(ViewNumber x, ViewNumber y) => double.MinMagnitude(x.Value, y.Value);

    public static ViewNumber MinMagnitudeNumber(ViewNumber x, ViewNumber y) => double.MinMagnitudeNumber(x.Value, y.Value);

    public static ViewNumber Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) => s.ToString();

    public static ViewNumber Parse(string s, NumberStyles style, IFormatProvider? provider) => s;

    public static ViewNumber Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => s.ToString();

    public static ViewNumber Parse(string s, IFormatProvider? provider) => s;

    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out ViewNumber result)
    {
        throw new NotImplementedException();
    }

    public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out ViewNumber result)
    {
        throw new NotImplementedException();
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out ViewNumber result)
    {
        throw new NotImplementedException();
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out ViewNumber result)
    {
        throw new NotImplementedException();
    }

    static ViewNumber INumberBase<ViewNumber>.Abs(ViewNumber value)
    {
        return double.Abs(value.Value);
    }

    static bool INumberBase<ViewNumber>.IsCanonical(ViewNumber value) => false;
    static bool INumberBase<ViewNumber>.IsComplexNumber(ViewNumber value) => false;

    static bool INumberBase<ViewNumber>.IsEvenInteger(ViewNumber value)
    {
        return double.IsEvenInteger(value.Value);
    }

    static bool INumberBase<ViewNumber>.IsFinite(ViewNumber value)
    {
        return double.IsFinite(value.Value);
    }

    static bool INumberBase<ViewNumber>.IsImaginaryNumber(ViewNumber value) => false;

    static bool INumberBase<ViewNumber>.IsInfinity(ViewNumber value)
    {
        return double.IsInfinity(value.Value);
    }

    static bool INumberBase<ViewNumber>.IsInteger(ViewNumber value)
    {
        return double.IsInteger(value.Value);
    }

    static bool INumberBase<ViewNumber>.IsNaN(ViewNumber value)
    {
        return double.IsNaN(value.Value);
    }

    static bool INumberBase<ViewNumber>.IsNegative(ViewNumber value)
    {
        return double.IsNegative(value.Value);
    }

    static bool INumberBase<ViewNumber>.IsNegativeInfinity(ViewNumber value)
    {
        return double.IsNegativeInfinity(value.Value);
    }

    static bool INumberBase<ViewNumber>.IsNormal(ViewNumber value)
    {
        return double.IsNormal(value.Value);
    }

    static bool INumberBase<ViewNumber>.IsOddInteger(ViewNumber value)
    {
        return double.IsOddInteger(value.Value);
    }

    static bool INumberBase<ViewNumber>.IsPositive(ViewNumber value)
    {
        return double.IsPositive(value.Value);
    }

    static bool INumberBase<ViewNumber>.IsPositiveInfinity(ViewNumber value)
    {
        return double.IsPositiveInfinity(value.Value);
    }

    static bool INumberBase<ViewNumber>.IsRealNumber(ViewNumber value)
    {
        return double.IsRealNumber(value.Value);
    }

    static bool INumberBase<ViewNumber>.IsSubnormal(ViewNumber value)
    {
        return double.IsSubnormal(value.Value);
    }

    static bool INumberBase<ViewNumber>.IsZero(ViewNumber value) => value.Value == 0;

    static ViewNumber INumberBase<ViewNumber>.MaxMagnitude(ViewNumber x, ViewNumber y)
    {
        if (x.Kind == y.Kind)
            return new(double.MaxMagnitude(x.Value, y.Value), x.Kind);
        else
            throw new Exception($"Cannot apply function with {x.Kind} and {y.Kind}");
    }

    static ViewNumber INumberBase<ViewNumber>.MaxMagnitudeNumber(ViewNumber x, ViewNumber y)
    {
        if (x.Kind == y.Kind)
            return new(double.MaxMagnitudeNumber(x.Value, y.Value), x.Kind);
        else
            throw new Exception($"Cannot apply function with {x.Kind} and {y.Kind}");
    }

    static ViewNumber INumberBase<ViewNumber>.MinMagnitude(ViewNumber x, ViewNumber y)
    {
        if (x.Kind == y.Kind)
            return new(double.MinMagnitude(x.Value, y.Value), x.Kind);
        else
            throw new Exception($"Cannot apply function with {x.Kind} and {y.Kind}");
    }

    static ViewNumber INumberBase<ViewNumber>.MinMagnitudeNumber(ViewNumber x, ViewNumber y)
    {
        if (x.Kind == y.Kind)
            return new(double.MinMagnitudeNumber(x.Value, y.Value), x.Kind);
        else
            throw new Exception($"Cannot apply function with {x.Kind} and {y.Kind}");
    }

    static ViewNumber INumberBase<ViewNumber>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
    {
        return s.ToString();
    }

    static ViewNumber INumberBase<ViewNumber>.Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
        return s;
    }

    static ViewNumber ISpanParsable<ViewNumber>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return s.ToString();
    }

    static ViewNumber IParsable<ViewNumber>.Parse(string s, IFormatProvider? provider)
    {
        return s;
    }

    static bool INumberBase<ViewNumber>.TryConvertFromChecked<TOther>(TOther value, out ViewNumber result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<ViewNumber>.TryConvertFromSaturating<TOther>(TOther value, out ViewNumber result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<ViewNumber>.TryConvertFromTruncating<TOther>(TOther value, out ViewNumber result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<ViewNumber>.TryConvertToChecked<TOther>(ViewNumber value, out TOther result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<ViewNumber>.TryConvertToSaturating<TOther>(ViewNumber value, out TOther result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<ViewNumber>.TryConvertToTruncating<TOther>(ViewNumber value, out TOther result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<ViewNumber>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out ViewNumber result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<ViewNumber>.TryParse(string? s, NumberStyles style, IFormatProvider? provider, out ViewNumber result)
    {
        throw new NotImplementedException();
    }

    static bool ISpanParsable<ViewNumber>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out ViewNumber result)
    {
        throw new NotImplementedException();
    }

    static bool IParsable<ViewNumber>.TryParse(string? s, IFormatProvider? provider, out ViewNumber result)
    {
        throw new NotImplementedException();
    }

    public int CompareTo(object? obj)
    {
        if (obj is ViewNumber other && Kind == other.Kind)
            return other.Value.CompareTo(Value);
        else throw new NotImplementedException();
    }

    public int CompareTo(ViewNumber other)
    {
        if (Kind == other.Kind)
            return other.Value.CompareTo(Value);
        else throw new NotImplementedException();
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var suffix = Kind switch
        {
            ViewNumberKind.Degree => "deg",
            ViewNumberKind.Percentage => "%",
            _ => ""
        };
        return $"{Value}{suffix}";
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    int IComparable.CompareTo(object? obj)
    {
        throw new NotImplementedException();
    }

    int IComparable<ViewNumber>.CompareTo(ViewNumber other)
    {
        throw new NotImplementedException();
    }

    bool IEquatable<ViewNumber>.Equals(ViewNumber other)
    {
        throw new NotImplementedException();
    }

    string IFormattable.ToString(string? format, IFormatProvider? formatProvider)
    {
        return ToString();
    }

    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    public static ViewNumber operator +(ViewNumber value)
    {
        return new(+value.Value, value.Kind);
    }

    static ViewNumber IUnaryPlusOperators<ViewNumber, ViewNumber>.operator +(ViewNumber value)
    {
        return new(+value.Value, value.Kind);
    }

    public static ViewNumber operator +(ViewNumber left, ViewNumber right)
    {
        if (left.Kind == right.Kind)
            return new(left.Value + right.Value, left.Kind);
        else throw new Exception($"Cannot add {left.Kind} with {right.Kind}");
    }

    static ViewNumber IAdditionOperators<ViewNumber, ViewNumber, ViewNumber>.operator +(ViewNumber left, ViewNumber right)
    {
        if (left.Kind == right.Kind)
            return new(left.Value + right.Value, left.Kind);
        else throw new Exception($"Cannot add {left.Kind} with {right.Kind}");
    }

    public static ViewNumber operator -(ViewNumber value)
    {
        return new(+value.Value, value.Kind);
    }

    static ViewNumber IUnaryNegationOperators<ViewNumber, ViewNumber>.operator -(ViewNumber value)
    {
        return new(+value.Value, value.Kind);
    }

    public static ViewNumber operator -(ViewNumber left, ViewNumber right)
    {
        if (left.Kind == right.Kind)
            return new(left.Value - right.Value, left.Kind);
        else throw new Exception($"Cannot subtract {left.Kind} with {right.Kind}");
    }

    static ViewNumber ISubtractionOperators<ViewNumber, ViewNumber, ViewNumber>.operator -(ViewNumber left, ViewNumber right)
    {
        if (left.Kind == right.Kind)
            return new(left.Value - right.Value, left.Kind);
        else throw new Exception($"Cannot subtract {left.Kind} with {right.Kind}");
    }

    public static ViewNumber operator ++(ViewNumber value)
    {
        return new(++value.Value, value.Kind);
    }

    static ViewNumber IIncrementOperators<ViewNumber>.operator ++(ViewNumber value)
    {
        return new(++value.Value, value.Kind);
    }

    public static ViewNumber operator --(ViewNumber value)
    {
        return new(--value.Value, value.Kind);
    }

    static ViewNumber IDecrementOperators<ViewNumber>.operator --(ViewNumber value)
    {
        return new(--value.Value, value.Kind);
    }

    public static ViewNumber operator *(ViewNumber left, ViewNumber right)
    {
        if (left.Kind == right.Kind)
            return new(left.Value * right.Value, left.Kind);
        else throw new Exception($"Cannot multiply {left.Kind} with {right.Kind}");
    }

    static ViewNumber IMultiplyOperators<ViewNumber, ViewNumber, ViewNumber>.operator *(ViewNumber left, ViewNumber right)
    {
        if (left.Kind == right.Kind)
            return new(left.Value * right.Value, left.Kind);
        else throw new Exception($"Cannot multiply {left.Kind} with {right.Kind}");
    }

    public static ViewNumber operator /(ViewNumber left, ViewNumber right)
    {
        if (left.Kind == right.Kind)
            return new(left.Value / right.Value, left.Kind);
        else throw new Exception($"Cannot divide {left.Kind} with {right.Kind}");
    }

    static ViewNumber IDivisionOperators<ViewNumber, ViewNumber, ViewNumber>.operator /(ViewNumber left, ViewNumber right)
    {
        if (left.Kind == right.Kind)
            return new(left.Value / right.Value, left.Kind);
        else throw new Exception($"Cannot divide {left.Kind} with {right.Kind}");
    }

    public static ViewNumber operator %(ViewNumber left, ViewNumber right)
    {
        if (right.Kind == ViewNumberKind.Number)
            return new(left.Value % right.Value, left.Kind);
        else throw new Exception($"Cannot mod {left.Kind} with {right.Kind}");
    }

    static ViewNumber IModulusOperators<ViewNumber, ViewNumber, ViewNumber>.operator %(ViewNumber left, ViewNumber right)
    {
        if (right.Kind == ViewNumberKind.Number)
            return new(left.Value % right.Value, left.Kind);
        else throw new Exception($"Cannot mod {left.Kind} with {right.Kind}");
    }

    static bool IEqualityOperators<ViewNumber, ViewNumber, bool>.operator ==(ViewNumber left, ViewNumber right)
    {
        return left.Value == right.Value && left.Kind == right.Kind;
    }

    static bool IEqualityOperators<ViewNumber, ViewNumber, bool>.operator !=(ViewNumber left, ViewNumber right)
    {
        return left.Value != right.Value || left.Kind != right.Kind;
    }

    public static bool operator <(ViewNumber left, ViewNumber right)
    {
        if (left.Kind == right.Kind)
            return left.Value < right.Value;
        else throw new Exception($"Cannot compare {left.Kind} with {right.Kind}");
    }

    static bool IComparisonOperators<ViewNumber, ViewNumber, bool>.operator <(ViewNumber left, ViewNumber right)
    {
        if (left.Kind == right.Kind)
            return left.Value < right.Value;
        else throw new Exception($"Cannot compare {left.Kind} with {right.Kind}");
    }

    public static bool operator >(ViewNumber left, ViewNumber right)
    {
        if (left.Kind == right.Kind)
            return left.Value > right.Value;
        else throw new Exception($"Cannot compare {left.Kind} with {right.Kind}");
    }

    static bool IComparisonOperators<ViewNumber, ViewNumber, bool>.operator >(ViewNumber left, ViewNumber right)
    {
        if (left.Kind == right.Kind)
            return left.Value > right.Value;
        else throw new Exception($"Cannot compare {left.Kind} with {right.Kind}");
    }

    public static bool operator <=(ViewNumber left, ViewNumber right)
    {
        if (left.Kind == right.Kind)
            return left.Value <= right.Value;
        else throw new Exception($"Cannot compare {left.Kind} with {right.Kind}");
    }

    static bool IComparisonOperators<ViewNumber, ViewNumber, bool>.operator <=(ViewNumber left, ViewNumber right)
    {
        if (left.Kind == right.Kind)
            return left.Value <= right.Value;
        else throw new Exception($"Cannot compare {left.Kind} with {right.Kind}");
    }

    public static bool operator >=(ViewNumber left, ViewNumber right)
    {
        if (left.Kind == right.Kind)
            return left.Value >= right.Value;
        else throw new Exception($"Cannot compare {left.Kind} with {right.Kind}");
    }

    static bool IComparisonOperators<ViewNumber, ViewNumber, bool>.operator >=(ViewNumber left, ViewNumber right)
    {
        if (left.Kind == right.Kind)
            return left.Value >= right.Value;
        else throw new Exception($"Cannot compare {left.Kind} with {right.Kind}");
    }

    public override string ToString()
    {
        var suffix = Kind switch
        {
            ViewNumberKind.Degree => "deg",
            ViewNumberKind.Percentage => "%",
            _ => ""
        };
        return $"{Value}{suffix}";
    }

    public static implicit operator ViewNumber(string s)
    {
        return s switch
        {
            string v when s.EndsWith('%') => new(double.Parse(v.AsSpan()[..^1]), ViewNumberKind.Percentage),
            string v when s.EndsWith("deg") => new(double.Parse(v.AsSpan()[..^1]), ViewNumberKind.Degree),
            string v => new(double.Parse(v), ViewNumberKind.Number)
        };
    }
    public static implicit operator ViewNumber(double s)
        => new(s, ViewNumberKind.Number);
    public static implicit operator ViewNumber(int s)
        => new(s, ViewNumberKind.Number);

    public static explicit operator float(ViewNumber vn) => (float)vn.Value;

}