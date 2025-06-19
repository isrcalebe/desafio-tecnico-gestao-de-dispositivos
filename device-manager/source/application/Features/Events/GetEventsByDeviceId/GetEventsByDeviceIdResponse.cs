using DeviceManager.Domain.Entities;

namespace DeviceManager.Application.Features.Events.GetEventsByDeviceId;

public record EventResponse(Guid EventId, Event.EventType Type, DateTime CreatedAt);

public record GetEventsByDeviceIdResponse(IEnumerable<EventResponse> events);
