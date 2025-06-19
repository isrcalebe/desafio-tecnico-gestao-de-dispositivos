using DeviceManager.Common;
using DeviceManager.Domain;
using Mediator;

namespace DeviceManager.Application.Features.Devices.Commands.UpdateDevice;

public record UpdateDeviceRequest(Guid DeviceId, string? SerialNumber, string? Imei) : IRequest<Result<UpdateDeviceResponse, Error>>;
