using DeviceManager.Common;
using DeviceManager.Domain;
using DeviceManager.Domain.Repositories;
using Mediator;

namespace DeviceManager.Application.Features.Clients.Queries.GetClientById;

public sealed class GetClientByIdHandler : IRequestHandler<GetClientByIdQuery, Result<GetClientByIdResponse, Error>>
{
    private readonly IClientRepository clientRepository;

    public GetClientByIdHandler(IClientRepository clientRepository)
    {
        this.clientRepository = clientRepository;
    }

    public async ValueTask<Result<GetClientByIdResponse, Error>> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetByIdAsync(request.Id, cancellationToken);

        if (client is null)
            return new Error("Client not found.", $"Client with ID {request.Id} does not exist.");

        return new GetClientByIdResponse(
            client.Id,
            client.Name,
            client.Email,
            client.Phone,
            client.Status
        );
    }
}
