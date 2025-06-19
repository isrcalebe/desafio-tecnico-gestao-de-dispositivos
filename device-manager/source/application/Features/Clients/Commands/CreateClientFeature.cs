using DeviceManager.Common;
using DeviceManager.Domain;
using DeviceManager.Domain.Entities;
using DeviceManager.Domain.Repositories;
using Mediator;

namespace DeviceManager.Application.Features.Clients.Commands;

public static class CreateClientFeature
{
    public record Request(string Name, string Email, string? Phone) : IRequest<Result<Guid, Error>>;

    public class Handler : IRequestHandler<Request, Result<Guid, Error>>
    {
        private readonly IClientRepository clientRepository;

        public Handler(IClientRepository clientRepository)
        {
            this.clientRepository = clientRepository;
        }

        public async ValueTask<Result<Guid, Error>> Handle(Request request, CancellationToken cancellationToken)
        {
            var clientExists = await clientRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (clientExists is not null)
                return new Error("Client with this email already exists.");

            var result = Client.Create(request.Name, request.Email, request.Phone);

            if (result.IsFailure)
                return new Error(result.Error.Message);

            var client = result.Value;

            await clientRepository.AddAsync(client, cancellationToken);

            return client.Id;
        }
    }
}
