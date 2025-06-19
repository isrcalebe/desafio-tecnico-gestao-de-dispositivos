using DeviceManager.Common;
using DeviceManager.Domain;
using Mediator;

namespace DeviceManager.Application.Features.Clients.Queries.GetAllClients;

public record GetAllClientsQuery() : IRequest<Result<GetAllClientsResponse, Error>>;
