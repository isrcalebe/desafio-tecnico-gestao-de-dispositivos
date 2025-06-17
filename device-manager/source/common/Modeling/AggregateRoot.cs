using DeviceManager.Common.Modeling.Events;

namespace DeviceManager.Common.Modeling;

/// <summary>
/// Represents the base class for an aggregate root in Domain-Driven Design (DDD).
/// </summary>
public abstract class AggregateRoot : Entity
{
    private readonly List<IDomainEvent> domainEvents = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateRoot"/> class with a specified identifier.
    /// </summary>
    /// <param name="id">
    /// The unique identifier for the aggregate root.
    /// </param>
    protected AggregateRoot(Guid id) : base(id) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateRoot"/> class without an identifier.
    /// </summary>
    protected AggregateRoot()
    {
    }

    /// <summary>
    /// Gets the list of domain events associated with this aggregate root.
    /// </summary>
    public IReadOnlyList<IDomainEvent> DomainEvents
        => domainEvents.AsReadOnly();

    /// <summary>
    /// Adds a domain event to the aggregate root.
    /// </summary>
    /// <param name="domainEvent">
    /// The domain event to add.
    /// </param>
    protected void AddDomainEvent(IDomainEvent domainEvent)
        => domainEvents.Add(domainEvent);

    /// <summary>
    /// Clears all domain events associated with this aggregate root.
    /// </summary>
    public void ClearDomainEvents()
        => domainEvents.Clear();
}
