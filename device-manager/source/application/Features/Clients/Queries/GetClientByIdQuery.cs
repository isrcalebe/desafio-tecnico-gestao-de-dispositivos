using DeviceManager.Common;
using DeviceManager.Domain.Repositories;
using Mediator;

namespace  DeviceManager.Application.Features.Clients.Queries;

public static class GetClientByIdQuery
{
    public record Query(Guid Id) : IRequest<Result<Response?, Error>>;

    public record Response(Guid Id, string Name, string Email, string? Phone, bool Status);

    public class Handler : IRequestHandler<Query, Result<Response?, Error>>
    {
        private readonly IClientRepository clientRepository;

        public Handler(IClientRepository clientRepository)
        {
            this.clientRepository = clientRepository;
        }

        public async ValueTask<Result<Response?, Error>> Handle(Query request, CancellationToken cancellationToken)
        {
            var client = await clientRepository.GetByIdAsync(request.Id, cancellationToken);
            if (client is null)
                return new Error("Client not found.");

            return new Response(client.Id, client.Name, client.Email, client.Phone, client.Status);
        }
    }

    public record Error(string ErrorMessage);
}
