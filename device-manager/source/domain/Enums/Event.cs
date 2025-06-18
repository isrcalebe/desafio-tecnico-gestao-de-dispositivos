using DeviceManager.Common.Modeling;
using DeviceManager.Domain.Entities;

namespace DeviceManager.Domain.Enums;

public class Event : Entity
{
    public Guid DeviceId { get; set; }

    public EventType Type { get; set; }

    public virtual Device? Device { get; set; }

    public new DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Event()
        : base(Guid.CreateVersion7())
    {
    }

    public enum EventType
    {
        PoweredOn,
        PoweredOff,
        Motion,
        SignalLoss
    }
}
