using System.Text.Json.Serialization;
using DeviceManager.Common.Modeling;

namespace DeviceManager.Domain.Entities;

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

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EventType : byte
    {
        PoweredOn,
        PoweredOff,
        Motion,
        SignalLoss
    }
}
