using DeviceManager.Domain.Entities;
using DeviceManager.Domain.Repositories;
using DeviceManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DeviceManager.Infrastructure.Repositories;

public sealed class ClientRepository : IClientRepository
{
    private readonly AppDbContext db;

    public ClientRepository(AppDbContext db)
    {
        this.db = db;
    }

    public async Task AddAsync(Client client, CancellationToken cancellationToken = default)
    {
        client.Status = true;

        await db.Clients.AddAsync(client, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Client client, CancellationToken cancellationToken = default)
    {
        db.Entry(client).State = EntityState.Modified;

        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var client = await db.Clients.FindAsync([id], cancellationToken);

        if (client is not null)
        {
            client.Status = false;
            db.Entry(client).State = EntityState.Modified;

            await db.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<IEnumerable<Client>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await db.Clients
            .Where(c => c.Status)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task<Client?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return db.Clients
            .FirstOrDefaultAsync(c => c.Email == email && c.Status, cancellationToken);
    }

    public async Task<Client?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await db.Clients
            .FindAsync([id], cancellationToken);
    }
}
