using DeviceManager.Common;
using DeviceManager.Domain;
using Mediator;

namespace DeviceManager.Application.Features.Devices.Commands.DeleteDevice;

public record DeleteDeviceRequest(Guid DeviceId) : IRequest<Result<DeleteDeviceResponse, Error>>;
