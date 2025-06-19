using DeviceManager.Common;
using DeviceManager.Domain;
using DeviceManager.Domain.Entities;
using DeviceManager.Domain.Repositories;
using Mediator;

namespace DeviceManager.Application.Features.Events.Commands.CreateEvent;

public sealed class CreateEventHandler : IRequestHandler<CreateEventRequest, Result<CreateEventResponse, Error>>
{
    private readonly IEventRepository eventRepository;
    private readonly IDeviceRepository deviceRepository;

    public CreateEventHandler(IEventRepository eventRepository, IDeviceRepository deviceRepository)
    {
        this.eventRepository = eventRepository;
        this.deviceRepository = deviceRepository;
    }

    public async ValueTask<Result<CreateEventResponse, Error>> Handle(CreateEventRequest request, CancellationToken cancellationToken)
    {
        var device = await deviceRepository.GetByIdAsync(request.DeviceId, cancellationToken);
        if (device is null)
            return new Error("Device not found", $"Device with ID {request.DeviceId} does not exist.");

        var @event = new Event
        {
            DeviceId = request.DeviceId,
            Type = request.Type,
            CreatedAt = DateTime.UtcNow
        };

        await eventRepository.AddAsync(@event, cancellationToken);

        return new CreateEventResponse(@event.Id);
    }
}
