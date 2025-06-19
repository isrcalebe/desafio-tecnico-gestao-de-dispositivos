using DeviceManager.Domain.Entities;
using DeviceManager.Domain.ValueObjects;
using DeviceManager.Infrastructure.Repositories;

namespace DeviceManager.IntegrationTests.Repositories;

public sealed class EventRepositoryTestScene : DbTestScene, IClassFixture<DbFixture>
{
    private readonly EventRepository eventRepository;
    private readonly ClientRepository clientRepository;
    private readonly DeviceRepository deviceRepository;

    public EventRepositoryTestScene(DbFixture fixture)
        : base(fixture)
    {
        eventRepository = new EventRepository(Db);
        clientRepository = new ClientRepository(Db);
        deviceRepository = new DeviceRepository(Db);
    }

    private static Event createEvent(Guid deviceId, DateTime createdAt)
    {
        return new Event
        {
            DeviceId = deviceId,
            CreatedAt = createdAt,
        };
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

    private async Task<Device> createAndPersistDeviceAsync()
    {
        var client = createClient();
        await clientRepository.AddAsync(client);

        var random = new Random();
        var randomIMEI = string.Concat(Enumerable.Range(0, 15).Select(_ => random.Next(0, 10).ToString(CultureInfo.InvariantCulture)));

        var manufacturerCode = new[] { "ABC", "XYZ", "DEF", "GHI", "JKL" }[random.Next(0, 5)];
        var serialNumber = SerialNumber.CreateManufacturer(manufacturerCode).Value;
        var imeiNumber = IMEI.Create(randomIMEI).Value;

        var device = Device.Create(serialNumber.Value, imeiNumber, client.Id).Value;
        await deviceRepository.AddAsync(device);

        return device;
    }

    [Fact]
    public async Task AddAsync_ShouldAddEventToDatabase()
    {
        var device = await createAndPersistDeviceAsync();
        var @event = createEvent(device.Id, DateTime.UtcNow);
        await eventRepository.AddAsync(@event);

        var savedEvent = await Db.Events.FindAsync(@event.Id);

        Assert.NotNull(savedEvent);
        Assert.Equal(@event.DeviceId, savedEvent.DeviceId);
    }

    [Fact]
    public async Task GetByDeviceIdAsync_ShouldReturnEventsWithinDateRange()
    {
        var device = await createAndPersistDeviceAsync();
        var anotherDevice = await createAndPersistDeviceAsync();
        var now = DateTime.UtcNow;
        var event1 = createEvent(device.Id, now.AddDays(-2));
        var event2 = createEvent(device.Id, now.AddDays(-1));
        var event3 = createEvent(anotherDevice.Id, now);
        await eventRepository.AddAsync(event1);
        await eventRepository.AddAsync(event2);
        await eventRepository.AddAsync(event3);

        var result = await eventRepository.GetByDeviceIdAsync(device.Id, now.AddDays(-3), now);

        Assert.Contains(result, e => e.Id == event1.Id);
        Assert.Contains(result, e => e.Id == event2.Id);
        Assert.DoesNotContain(result, e => e.Id == event3.Id);
    }

    [Fact]
    public async Task GetByDeviceIdAsync_ShouldReturnEventsOrderedByCreatedAtDescending()
    {
        var device = await createAndPersistDeviceAsync();
        var now = DateTime.UtcNow;
        var event1 = createEvent(device.Id, now.AddHours(-2));
        var event2 = createEvent(device.Id, now.AddHours(-1));
        Db.Events.AddRange(event1, event2);
        await Db.SaveChangesAsync();

        var result = (await eventRepository.GetByDeviceIdAsync(device.Id, now.AddDays(-1), now.AddDays(1))).ToList();

        Assert.Equal(event2.Id, result[0].Id);
        Assert.Equal(event1.Id, result[1].Id);
    }

    [Fact]
    public async Task GetEventsFromLastDaysAsync_ShouldReturnEventsFromLastNDays()
    {
        var device = await createAndPersistDeviceAsync();
        var now = DateTime.UtcNow;
        var eventOld = createEvent(device.Id, now.AddDays(-10));
        var eventRecent = createEvent(device.Id, now.AddDays(-1));
        Db.Events.AddRange(eventOld, eventRecent);
        await Db.SaveChangesAsync();

        var result = await eventRepository.GetEventsFromLastDaysAsync(5);

        Assert.Contains(result, e => e.Id == eventRecent.Id);
        Assert.DoesNotContain(result, e => e.Id == eventOld.Id);
    }

    [Fact]
    public async Task GetEventsFromLastDaysAsync_ShouldReturnEventsOrderedByCreatedAtDescending()
    {
        var device = await createAndPersistDeviceAsync();
        var now = DateTime.UtcNow;
        var event1 = createEvent(device.Id, now.AddHours(-2));
        var event2 = createEvent(device.Id, now.AddHours(-1));
        Db.Events.AddRange(event1, event2);
        await Db.SaveChangesAsync();

        var result = (await eventRepository.GetEventsFromLastDaysAsync(1)).ToList();

        Assert.True(result.Count >= 2);
        Assert.Equal(event2.Id, result[0].Id);
        Assert.Equal(event1.Id, result[1].Id);
    }
}
