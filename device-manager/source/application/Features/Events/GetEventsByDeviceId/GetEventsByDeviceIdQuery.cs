using DeviceManager.Common;
using DeviceManager.Domain;
using Mediator;

namespace DeviceManager.Application.Features.Events.GetEventsByDeviceId;

public record GetEventsByDeviceIdQuery(Guid DeviceId, DateTime From, DateTime To) : IRequest<Result<GetEventsByDeviceIdResponse, Error>>;
