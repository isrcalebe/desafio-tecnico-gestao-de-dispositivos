using DeviceManager.Common;
using DeviceManager.Domain;
using Mediator;

namespace DeviceManager.Application.Features.Devices.Queries.GetDevicesByClientId;

public record GetDevicesByClientIdQuery(Guid ClientId) : IRequest<Result<GetDevicesByClientIdResponse, Error>>;
