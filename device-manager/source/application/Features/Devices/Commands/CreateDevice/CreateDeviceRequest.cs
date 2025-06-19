using DeviceManager.Common;
using DeviceManager.Domain;
using Mediator;

namespace DeviceManager.Application.Features.Devices.Commands.CreateDevice;

public record CreateDeviceRequest(
    Guid ClientId,
    string SerialNumber,
    string IMEI,
    DateTime? ActivatedAt
) : IRequest<Result<CreateDeviceResponse, Error>>;
