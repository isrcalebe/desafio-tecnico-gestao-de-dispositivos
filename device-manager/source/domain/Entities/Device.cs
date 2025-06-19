using DeviceManager.Common.Modeling;

namespace DeviceManager.Domain.Entities;

public class Device : AggregateRoot
{
    public string Serial { get; set; } = string.Empty;

    public string IMEI { get; set; } = string.Empty;

    public DateTime? ActivatedAt { get; set; }

    public Guid ClientId { get; set; }

    public virtual Client? Client { get; set; }

    public virtual ICollection<Event> Events { get; set; } = [];

    private Device()
        : base(Guid.CreateVersion7())
    {
    }

    public static Device Create(string serial, string imei, Guid clientId)
        => new()
        {
            Serial = serial,
            IMEI = imei,
            ClientId = clientId
        };
}
