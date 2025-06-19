using DeviceManager.Common;
using DeviceManager.Domain;
using DeviceManager.Domain.Repositories;
using Mediator;

namespace DeviceManager.Application.Features.Events.GetEventsByDeviceId;

public sealed class GetEventsByDeviceIdHandler : IRequestHandler<GetEventsByDeviceIdQuery, Result<GetEventsByDeviceIdResponse, Error>>
{
    private readonly IEventRepository eventRepository;
    private readonly IDeviceRepository deviceRepository;

    public GetEventsByDeviceIdHandler(IEventRepository eventRepository, IDeviceRepository deviceRepository)
    {
        this.eventRepository = eventRepository;
        this.deviceRepository = deviceRepository;
    }

    public async ValueTask<Result<GetEventsByDeviceIdResponse, Error>> Handle(GetEventsByDeviceIdQuery request, CancellationToken cancellationToken)
    {
        var device = await deviceRepository.GetByIdAsync(request.DeviceId, cancellationToken);
        if (device is null)
            return new Error("Device not found", $"Device with ID {request.DeviceId} does not exist.");

        var events = await eventRepository.GetByDeviceIdAsync(device.Id, request.From, request.To, cancellationToken);

        var eventsResponse = events.Select(e => new EventResponse(
            e.Id,
            e.Type,
            e.CreatedAt
        ));

        return new GetEventsByDeviceIdResponse(eventsResponse);
    }
}
