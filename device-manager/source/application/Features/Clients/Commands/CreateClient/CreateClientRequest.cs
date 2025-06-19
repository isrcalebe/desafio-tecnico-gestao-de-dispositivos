using DeviceManager.Common;
using DeviceManager.Domain;
using Mediator;

namespace DeviceManager.Application.Features.Clients.Commands.CreateClient;

public record CreateClientRequest(
    string Name,
    string Email,
    string? Phone
) : IRequest<Result<CreateClientResponse, Error>>;
