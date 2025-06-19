using DeviceManager.Common;
using DeviceManager.Domain;
using DeviceManager.Domain.Entities;
using Mediator;

namespace DeviceManager.Application.Features.Events.Commands.CreateEvent;

public record CreateEventRequest(Event.EventType Type, Guid DeviceId) : IRequest<Result<CreateEventResponse, Error>>;
