using DeviceManager.Common;
using DeviceManager.Domain;
using DeviceManager.Domain.Entities;
using DeviceManager.Domain.Repositories;
using Mediator;

namespace DeviceManager.Application.Features.Clients.Commands.CreateClient;

public sealed class CreateClientHandler : IRequestHandler<CreateClientRequest, Result<CreateClientResponse, Error>>
{
    private readonly IClientRepository clientRepository;

    public CreateClientHandler(IClientRepository clientRepository)
    {
        this.clientRepository = clientRepository;
    }

    public async ValueTask<Result<CreateClientResponse, Error>> Handle(CreateClientRequest request, CancellationToken cancellationToken)
    {
        var clientExists = await clientRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (clientExists is not null)
            return new Error("Client with this email already exists.");

        var result = Client.Create(request.Name, request.Email, request.Phone);

        if (result.IsFailure)
            return result.Error;

        var client = result.Value;

        await clientRepository.AddAsync(client, cancellationToken);

        return new CreateClientResponse(client.Id, client.Name, client.Email);
    }
}
