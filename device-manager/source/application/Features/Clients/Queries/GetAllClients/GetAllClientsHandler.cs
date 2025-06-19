using DeviceManager.Application.Features.Clients.Queries.GetClientById;
using DeviceManager.Common;
using DeviceManager.Domain;
using DeviceManager.Domain.Repositories;
using Mediator;

namespace DeviceManager.Application.Features.Clients.Queries.GetAllClients;

public sealed class GetAllClientsHandler : IRequestHandler<GetAllClientsQuery, Result<GetAllClientsResponse, Error>>
{
    private readonly IClientRepository clientRepository;

    public GetAllClientsHandler(IClientRepository clientRepository)
    {
        this.clientRepository = clientRepository;
    }

    public async ValueTask<Result<GetAllClientsResponse, Error>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
    {
        var clients = await clientRepository.GetAllAsync(cancellationToken);

        if (clients is null || !clients.Any())
            return new Error("No clients found.", "There are no clients in the system.");

        var clientResponses = clients.Select(client => new GetClientByIdResponse(
            client.Id,
            client.Name,
            client.Email,
            client.Phone,
            client.Status
        ));

        return new GetAllClientsResponse(clientResponses);
    }
}
