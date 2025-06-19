using DeviceManager.Common;
using DeviceManager.Domain;
using DeviceManager.Domain.Repositories;
using Mediator;

namespace DeviceManager.Application.Features.Clients.Commands.DeleteClient;

public sealed class DeleteClientHandler : IRequestHandler<DeleteClientRequest, Result<DeleteClientResponse, Error>>
{
    private readonly IClientRepository clientRepository;

    public DeleteClientHandler(IClientRepository clientRepository)
    {
        this.clientRepository = clientRepository;
    }

    public async ValueTask<Result<DeleteClientResponse, Error>> Handle(DeleteClientRequest request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetByIdAsync(request.ClientId, cancellationToken);
        if (client is null)
            return new Error("Client not found", $"Client with ID {request.ClientId} does not exist.");

        await clientRepository.DeleteAsync(client, cancellationToken);

        return new DeleteClientResponse(true);
    }
}
