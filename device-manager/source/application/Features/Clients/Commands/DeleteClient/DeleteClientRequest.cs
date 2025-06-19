using DeviceManager.Common;
using DeviceManager.Domain;
using Mediator;

namespace DeviceManager.Application.Features.Clients.Commands.DeleteClient;

public record DeleteClientRequest(
    Guid ClientId
) : IRequest<Result<DeleteClientResponse, Error>>;
