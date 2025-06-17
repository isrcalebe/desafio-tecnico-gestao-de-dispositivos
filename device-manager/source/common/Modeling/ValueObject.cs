using System.Numerics;

namespace DeviceManager.Common.Modeling;

/// <summary>
/// Represents a base class for value objects, providing value-based equality comparison.
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>, IEqualityOperators<ValueObject, ValueObject, bool>
{
    /// <summary>
    /// Gets the components that define the value object for equality comparison.
    /// </summary>
    /// <returns>
    /// An enumerable collection of objects that represent the value components of the value object.
    /// </returns>
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj is null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }

    public bool Equals(ValueObject? other)
        => Equals((object?)other);

    public static bool operator ==(ValueObject? a, ValueObject? b)
    {
        if (a is null && b is null)
            return true;
        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(ValueObject? a, ValueObject? b)
        => !(a == b);
}
