using System.Numerics;

namespace DeviceManager.Common.Modeling;

/// <summary>
/// Represents a base class for entities in the system.
/// </summary>
public abstract class Entity : IEquatable<Entity>, IEqualityOperators<Entity, Entity, bool>
{
    /// <summary>
    /// Represents the unique identifier for the entity.
    /// </summary>
    public Guid Id { get; protected set; }

    /// <summary>
    /// Represents the date and time when the entity was created.
    /// </summary>
    public DateTime CreatedAt { get; protected set; }

    /// <summary>
    /// Represents the date and time when the entity was last updated.
    /// </summary>
    public DateTime LastUpdatedAt { get; protected set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Entity"/> class with a specified identifier.
    /// </summary>
    /// <param name="id">
    /// The unique identifier for the entity.
    /// </param>
    protected Entity(Guid id)
    {
        Id = id;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Entity"/> class without an identifier.
    /// </summary>
    protected Entity()
    {
    }

    public bool Equals(Entity? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj) => Equals(obj as Entity);

    public override int GetHashCode()
        => (GetType().ToString() + Id).GetHashCode();

    public static bool operator ==(Entity? left, Entity? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(Entity? left, Entity? right)
        => !(left == right);
}
