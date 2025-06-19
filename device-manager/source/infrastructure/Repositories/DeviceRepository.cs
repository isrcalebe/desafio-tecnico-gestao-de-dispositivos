using DeviceManager.Domain.Entities;
using DeviceManager.Domain.Repositories;
using DeviceManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DeviceManager.Infrastructure.Repositories;

public sealed class DeviceRepository : IDeviceRepository
{
    private readonly AppDbContext db;

    public DeviceRepository(AppDbContext db)
    {
        this.db = db;
    }

    public async Task AddAsync(Device device, CancellationToken cancellationToken = default)
    {
        await db.Devices.AddAsync(device, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Device device, CancellationToken cancellationToken = default)
    {
        db.Entry(device).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var device = await db.Devices.FindAsync([id], cancellationToken);

        if (device is not null)
        {
            db.Devices.Remove(device);
            await db.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<IEnumerable<Device>> GetByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default)
    {
        return await db.Devices
            .Where(d => d.ClientId == clientId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Device?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await db.Devices
            .FindAsync([id], cancellationToken);
    }

    public async Task<Device?> GetBySerialAsync(string serial, CancellationToken cancellationToken = default)
    {
        return await db.Devices
            .FirstOrDefaultAsync(d => d.SerialNumber.Value == serial, cancellationToken);
    }

    public async Task<IEnumerable<Device>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await db.Devices
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
