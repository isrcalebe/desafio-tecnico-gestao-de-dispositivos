using DeviceManager.Common;
using DeviceManager.Domain;
using Mediator;

namespace DeviceManager.Application.Features.Devices.Queries.GetDeviceById;

public record GetDeviceByIdQuery(Guid Id) : IRequest<Result<GetDeviceByIdResponse, Error>>;
