using DeviceManager.Common;
using DeviceManager.Domain;
using Mediator;

namespace DeviceManager.Application.Features.Clients.Commands.UpdateClient;

public record UpdateClientRequest(
    Guid ClientId,
    string Name,
    string Email,
    string? Phone,
    bool Status
) : IRequest<Result<UpdateClientResponse, Error>>;
