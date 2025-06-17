using DeviceManager.Domain.Entities;

namespace DeviceManager.Domain.Repositories;

public interface IClientRepository
{
    Task<Client?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Client?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<IEnumerable<Client>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Client client, CancellationToken cancellationToken = default);

    Task UpdateAsync(Client client, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
