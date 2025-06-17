using DeviceManager.Domain.Enums;

namespace DeviceManager.Domain.Repositories;

public interface IEventRepository
{
    Task<IEnumerable<Event>> GetByDeviceIdAsync(Guid deviceId, DateTime start, DateTime end, CancellationToken cancellationToken = default);

    Task<IEnumerable<Event>> GetEventsFromLastDaysAsync(int days, CancellationToken cancellationToken = default);

    Task AddAsync(Event @event, CancellationToken cancellationToken = default);
}
