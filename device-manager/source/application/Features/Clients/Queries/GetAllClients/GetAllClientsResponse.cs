using DeviceManager.Application.Features.Clients.Queries.GetClientById;

namespace DeviceManager.Application.Features.Clients.Queries.GetAllClients;

public record GetAllClientsResponse(
    IEnumerable<GetClientByIdResponse> Clients);
