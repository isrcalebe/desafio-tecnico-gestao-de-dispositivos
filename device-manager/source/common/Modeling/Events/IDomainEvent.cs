namespace DeviceManager.Common.Modeling.Events;

/// <summary>
/// Represents a domain event in the system.
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// When the event occurred.
    /// </summary>
    DateTime OccurredOn { get; }
}
