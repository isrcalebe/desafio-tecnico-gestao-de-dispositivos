
using DeviceManager.Infrastructure.Persistence;
using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using Respawn;

namespace DeviceManager.IntegrationTests;

public abstract class DbTestScene : IAsyncLifetime
{
    protected AppDbContext Db { get; private set; } = null!;

    protected IContainer Container { get; private set; } = null!;

    protected Respawner Respawner { get; private set; } = null!;

    private readonly DbFixture fixture;

    protected DbTestScene(DbFixture fixture)
    {
        this.fixture = fixture;

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(fixture.ConnectionString)
            .Options;

        Db = new AppDbContext(options);
    }

    public async Task InitializeAsync()
    {
        Respawner = await Respawner.CreateAsync(fixture.ConnectionString, new RespawnerOptions
        {
            TablesToIgnore =
            [
                "__EFMigrationsHistory"
            ]
        });

        await Respawner.ResetAsync(fixture.ConnectionString);
        await SeedDataAsync(Db);
    }

    public async Task DisposeAsync()
    {
        await Db.DisposeAsync();
    }

    protected virtual Task SeedDataAsync(AppDbContext db)
    {
        return Task.CompletedTask;
    }
}
