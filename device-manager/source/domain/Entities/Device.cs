using DeviceManager.Common;
using DeviceManager.Common.Modeling;
using DeviceManager.Domain.ValueObjects;

namespace DeviceManager.Domain.Entities;

public class Device : AggregateRoot
{
    public SerialNumber SerialNumber { get; private set; }

    public IMEI IMEI { get; private set; }

    public DateTime? ActivatedAt { get; private set; }

    public Guid ClientId { get; private set; }

    public virtual Client? Client { get; private set; }

    public virtual ICollection<Event> Events { get; private set; } = [];

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Device()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        : base(Guid.CreateVersion7())
    {
    }

    public static Result<Device, Error> Create(string serial, string imei, Guid clientId)
    {
        var device = new Device();

        var serialResult = SerialNumber.Create(serial);
        if (serialResult.IsFailure)
            return serialResult.Error;

        var imeiResult = IMEI.Create(imei);
        if (imeiResult.IsFailure)
            return imeiResult.Error;

        device.SerialNumber = serialResult.Value;
        device.IMEI = imeiResult.Value;
        device.ClientId = clientId;

        return device;
    }

    public static Result<Device, Error> Create(SerialNumber serial, IMEI imei, Guid clientId)
    {
        var device = new Device
        {
            SerialNumber = serial,
            IMEI = imei,
            ClientId = clientId
        };

        return device;
    }

    public bool Activate()
    {
        if (ActivatedAt.HasValue)
            return false;

        var now = DateTime.UtcNow;

        ActivatedAt = now;
        LastUpdatedAt = now;
        return true;
    }

    public void UpdateSerialNumber(SerialNumber serialNumber)
    {
        SerialNumber = serialNumber;
        LastUpdatedAt = DateTime.UtcNow;
    }

    public void UpdateIMEI(IMEI imei)
    {
        IMEI = imei;
        LastUpdatedAt = DateTime.UtcNow;
    }

    public void UpdateClientId(Guid clientId)
    {
        ClientId = clientId;
        LastUpdatedAt = DateTime.UtcNow;
    }
}
