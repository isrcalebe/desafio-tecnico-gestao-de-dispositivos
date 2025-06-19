using DeviceManager.Domain.Entities;
using DeviceManager.Domain.ValueObjects;
using DeviceManager.Infrastructure.Repositories;

namespace DeviceManager.IntegrationTests.Repositories;

public sealed class ClientRepositoryTestScene : DbTestScene, IClassFixture<DbFixture>
{
    private readonly ClientRepository clientRepository;

    public ClientRepositoryTestScene(DbFixture fixture)
        : base(fixture)
    {
        clientRepository = new ClientRepository(Db);
    }

    private static Client createClient(Guid? id = null, string? email = null, bool status = true)
    {
        return Client.Create(
            "Test Client",
            email ?? $"test{Guid.CreateVersion7()}@mail.com",
            phone: "1234567890",
            status: status
        ).Value;
    }

    [Fact]
    public async Task AddAsync_ShouldAddClientWithStatusTrue()
    {
        var client = createClient(status: false);

        await clientRepository.AddAsync(client);
        var added = await Db.Clients.FindAsync(client.Id);

        Assert.NotNull(added);
        Assert.True(added!.Status);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateClient()
    {
        var client = createClient();

        Db.Clients.Add(client);
        Db.SaveChanges();
        client.UpdateName(ClientName.Create("Updated Name").Value);
        await clientRepository.UpdateAsync(client);
        var updated = await Db.Clients.FindAsync(client.Id);

        Assert.Equal("Updated Name", updated!.Name);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveClient()
    {
        var client = createClient();

        Db.Clients.Add(client);
        Db.SaveChanges();
        await clientRepository.DeleteAsync(client);
        var deleted = await Db.Clients.FindAsync(client.Id);

        Assert.Null(deleted);
    }


    [Fact]
    public async Task GetAllAsync_ShouldReturnOnlyActiveClients()
    {
        var activeClient = createClient(status: true);
        var inactiveClient = createClient(status: false);

        Db.Clients.AddRange(activeClient, inactiveClient);
        Db.SaveChanges();
        var result = await clientRepository.GetAllAsync();

        Assert.Single(result);
        Assert.Contains(result, c => c.Id == activeClient.Id);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnClient_WhenExistsAndActive()
    {
        var email = "unique@mail.com";
        var client = createClient(email: email, status: true);

        Db.Clients.Add(client);
        Db.SaveChanges();
        var result = await clientRepository.GetByEmailAsync(email);

        Assert.NotNull(result);
        Assert.Equal(email, result!.Email);
    }
    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNull_WhenClientIsInactive()
    {
        var email = "inactive@mail.com";
        var client = createClient(email: email, status: false);

        Db.Clients.Add(client);
        Db.SaveChanges();
        var result = await clientRepository.GetByEmailAsync(email);

        Assert.Null(result);
    }
    [Fact]
    public async Task GetByIdAsync_ShouldReturnClient_WhenExists()
    {
        var client = createClient();

        Db.Clients.Add(client);
        Db.SaveChanges();
        var result = await clientRepository.GetByIdAsync(client.Id);

        Assert.NotNull(result);
        Assert.Equal(client.Id, result!.Id);
    }
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        var result = await clientRepository.GetByIdAsync(Guid.NewGuid());
        Assert.Null(result);
    }
}
