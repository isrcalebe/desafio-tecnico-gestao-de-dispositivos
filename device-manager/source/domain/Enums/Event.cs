using DeviceManager.Common.Modeling;
using DeviceManager.Domain.Entities;

namespace DeviceManager.Domain.Enums;

public class Event : Entity
{
    public Guid DeviceId { get; set; }

    public EventType Type { get; set; }

    public virtual Device? Device { get; set; }

    private Event()
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
