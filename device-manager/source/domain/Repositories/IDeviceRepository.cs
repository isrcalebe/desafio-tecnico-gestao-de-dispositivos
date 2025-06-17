using DeviceManager.Domain.Entities;

namespace DeviceManager.Domain.Repositories;

public interface IDeviceRepository
{
    Task<Device?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Device?> GetBySerialAsync(string serial, CancellationToken cancellationToken = default);

    Task<Device?> GetByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default);

    Task AddAsync(Device device, CancellationToken cancellationToken = default);

    Task UpdateAsync(Device device, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
