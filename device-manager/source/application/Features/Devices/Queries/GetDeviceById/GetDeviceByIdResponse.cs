namespace DeviceManager.Application.Features.Devices.Queries.GetDeviceById;

public record GetDeviceByIdResponse(Guid DeviceId, string SerialNumber, string Imei, DateTime? ActivatedAt);
