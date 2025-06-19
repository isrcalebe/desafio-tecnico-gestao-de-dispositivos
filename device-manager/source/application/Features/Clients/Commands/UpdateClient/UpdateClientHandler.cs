using DeviceManager.Common;
using DeviceManager.Domain;
using DeviceManager.Domain.Repositories;
using DeviceManager.Domain.ValueObjects;
using Mediator;

namespace DeviceManager.Application.Features.Clients.Commands.UpdateClient;

public sealed class UpdateClientHandler : IRequestHandler<UpdateClientRequest, Result<UpdateClientResponse, Error>>
{
    private readonly IClientRepository clientRepository;

    public UpdateClientHandler(IClientRepository clientRepository)
    {
        this.clientRepository = clientRepository;
    }

    public async ValueTask<Result<UpdateClientResponse, Error>> Handle(UpdateClientRequest request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetByIdAsync(request.ClientId, cancellationToken);

        if (client is null)
            return new Error("Client not found", $"Client with ID {request.ClientId} does not exist.");

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var newClientName = ClientName.Create(request.Name);
            if (newClientName.IsFailure)
                return newClientName.Error;

            client.UpdateName(newClientName.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var newEmail = Email.Create(request.Email);
            if (newEmail.IsFailure)
                return newEmail.Error;

            client.UpdateEmail(newEmail.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Phone))
        {
            var newPhone = PhoneNumber.Create(request.Phone);
            if (newPhone.IsFailure)
                return newPhone.Error;

            client.UpdatePhone(newPhone.Value);
        }

        client.UpdateStatus(request.Status);

        await clientRepository.UpdateAsync(client, cancellationToken);

        return new UpdateClientResponse(true);
    }
}
