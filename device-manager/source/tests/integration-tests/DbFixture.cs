using DeviceManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace DeviceManager.IntegrationTests;

public sealed class DbFixture : IAsyncLifetime
{
    private readonly MsSqlContainer msSqlContainer;

    public string ConnectionString => msSqlContainer.GetConnectionString();

    public DbFixture()
    {
        msSqlContainer = new MsSqlBuilder()
            .WithCleanUp(true)
            .Build();
    }

    public async Task InitializeAsync()
    {
        await msSqlContainer.StartAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(ConnectionString)
            .Options;

        using (var context = new AppDbContext(options))
        {
            await context.Database.EnsureCreatedAsync();
            await context.Database.MigrateAsync();
        }
    }

    public async Task DisposeAsync()
    {
        await msSqlContainer.StopAsync();
        await msSqlContainer.DisposeAsync();
    }
}
