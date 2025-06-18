using DeviceManager.Domain.Entities;
using DeviceManager.Infrastructure.Repositories;

namespace DeviceManager.IntegrationTests.Repositories;

public sealed class DeviceRepositoryTestScene : DbTestScene, IClassFixture<DbFixture>
{
    private readonly DeviceRepository deviceRepository;
    private readonly ClientRepository clientRepository;

    public DeviceRepositoryTestScene(DbFixture fixture)
        : base(fixture)
    {
        deviceRepository = new DeviceRepository(Db);
        clientRepository = new ClientRepository(Db);
    }

    private static Client createClient(Guid? id = null, string? email = null, bool status = true)
    {
        return Client.Create(
            "Test Client",
            email ?? $"test{Guid.NewGuid()}@mail.com",
            phone: "1234567890",
            status: status
        );
    }

    private static Device createDevice(string? imei = null, string? serial = null, Guid? clientId = null)
    {
        return Device.Create(
            serial ?? "serial" + Guid.CreateVersion7().ToString("N"),
            imei ?? "imei" + Guid.CreateVersion7().ToString("N"),
            clientId ?? Guid.CreateVersion7()
        );
    }

    [Fact]
    public async Task AddAsync_ShouldAddDevice()
    {
        var client = createClient();
        await clientRepository.AddAsync(client);
        var device = createDevice(clientId: client.Id);
        await deviceRepository.AddAsync(device);

        Assert.Equal(1, Db.Devices.Count());
        Assert.Equal(device.Id, Db.Devices.First().Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateDevice()
    {
        var client = createClient();
        await clientRepository.AddAsync(client);
        var device = createDevice(clientId: client.Id);
        await deviceRepository.AddAsync(device);

        device.Serial = "UpdatedSerial";
        await deviceRepository.UpdateAsync(device);
        var updated = Db.Devices.Find(device.Id);

        Assert.Equal("UpdatedSerial", updated!.Serial);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveDevice()
    {
        var client = createClient();
        await clientRepository.AddAsync(client);
        var device = createDevice(clientId: client.Id);
        await deviceRepository.AddAsync(device);

        await deviceRepository.DeleteAsync(device.Id);

        Assert.Null(Db.Devices.Find(device.Id));
    }

    [Fact]
    public async Task GetByClientIdAsync_ShouldReturnDevicesForClient()
    {
        var client = createClient();
        var anotherClient = createClient();
        await clientRepository.AddAsync(client);
        await clientRepository.AddAsync(anotherClient);
        var device1 = createDevice(clientId: client.Id);
        var device2 = createDevice(clientId: client.Id);
        var device3 = createDevice(clientId: anotherClient.Id);
        await deviceRepository.AddAsync(device1);
        await deviceRepository.AddAsync(device2);
        await deviceRepository.AddAsync(device3);

        var result = await deviceRepository.GetByClientIdAsync(client.Id);

        Assert.Contains(result, d => d.Id == device1.Id);
        Assert.Contains(result, d => d.Id == device2.Id);
        Assert.DoesNotContain(result, d => d.Id == device3.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnDevice()
    {
        var client = createClient();
        await clientRepository.AddAsync(client);
        var device = createDevice(clientId: client.Id);
        await deviceRepository.AddAsync(device);

        var result = await deviceRepository.GetByIdAsync(device.Id);

        Assert.NotNull(result);
        Assert.Equal(device.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNullIfNotFound()
    {
        var result = await deviceRepository.GetByIdAsync(Guid.CreateVersion7());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetBySerialAsync_ShouldReturnDevice()
    {
        var client = createClient();
        await clientRepository.AddAsync(client);
        var device = createDevice(serial: "serial123", clientId: client.Id);
        await deviceRepository.AddAsync(device);

        var result = await deviceRepository.GetBySerialAsync("serial123");

        Assert.NotNull(result);
        Assert.Equal(device.Id, result.Id);
    }

    [Fact]
    public async Task GetBySerialAsync_ShouldReturnNullIfNotFound()
    {
        var result = await deviceRepository.GetBySerialAsync("notfound");

        Assert.Null(result);
    }
}
