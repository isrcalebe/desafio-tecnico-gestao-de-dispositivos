using DeviceManager.Domain.Entities;
using DeviceManager.Domain.Repositories;
using DeviceManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DeviceManager.Infrastructure.Repositories;

public sealed class EventRepository : IEventRepository
{
    private readonly AppDbContext db;

    public EventRepository(AppDbContext db)
    {
        this.db = db;
    }

    public async Task AddAsync(Event @event, CancellationToken cancellationToken = default)
    {
        await db.Events.AddAsync(@event, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Event>> GetByDeviceIdAsync(Guid deviceId, DateTime start, DateTime end, CancellationToken cancellationToken = default)
    {
        return await db.Events
            .Where(e => e.DeviceId == deviceId && e.CreatedAt >= start && e.CreatedAt <= end)
            .OrderByDescending(e => e.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Event>> GetEventsFromLastDaysAsync(int days, CancellationToken cancellationToken = default)
    {
        var startDate = DateTime.UtcNow.AddDays(-days);

        return await db.Events
            .Where(e => e.CreatedAt >= startDate)
            .OrderByDescending(e => e.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
