namespace DeviceManager.Application.Features.Clients.Queries.GetClientById;

public record GetClientByIdResponse(Guid ClientId, string Name, string Email, string? Phone, bool Status);
