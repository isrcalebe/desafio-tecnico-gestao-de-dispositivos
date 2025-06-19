using DeviceManager.Application.Features.Clients.Queries.GetClientById;
using DeviceManager.Common;
using DeviceManager.Domain;
using Mediator;

namespace DeviceManager.Application.Features.Clients.Queries.GetAllClients;

public record GetAllClientsResponse(
    IEnumerable<GetClientByIdResponse> Clients);
