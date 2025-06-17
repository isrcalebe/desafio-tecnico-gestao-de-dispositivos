using DeviceManager.Common.Modeling;
using DeviceManager.Common.Modeling.Events;

namespace DeviceManager.UnitTests.Common.Modeling;

public class AggregateRootTestScene
{
    public class DummyDomainEvent : IDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }

    public class DummyAggregateRoot : AggregateRoot
    {
        public DummyAggregateRoot() : base() { }

        public DummyAggregateRoot(Guid id) : base(id) { }

        public void AddEvent(IDomainEvent e) => AddDomainEvent(e);
    }


    [Fact]
    public void DomainEvents_ShouldBeEmpty_OnInitialization()
    {
        var aggregate = new DummyAggregateRoot();

        Assert.Empty(aggregate.DomainEvents);
    }

    [Fact]
    public void AddDomainEvent_ShouldAddEvent()
    {
        var aggregate = new DummyAggregateRoot();
        var domainEvent = new DummyDomainEvent();

        aggregate.AddEvent(domainEvent);

        Assert.Single(aggregate.DomainEvents);
        Assert.Contains(domainEvent, aggregate.DomainEvents);
    }

    [Fact]
    public void ClearDomainEvents_ShouldRemoveAllEvents()
    {
        var aggregate = new DummyAggregateRoot();

        aggregate.AddEvent(new DummyDomainEvent());
        aggregate.AddEvent(new DummyDomainEvent());

        aggregate.ClearDomainEvents();

        Assert.Empty(aggregate.DomainEvents);
    }

    [Fact]
    public void DomainEvents_ShouldBeReadOnly()
    {
        var aggregate = new DummyAggregateRoot();

        Assert.IsAssignableFrom<IReadOnlyList<IDomainEvent>>(aggregate.DomainEvents);
    }

    [Fact]
    public void AggregateRoot_CanBeConstructedWithId()
    {
        var id = Guid.NewGuid();
        var aggregate = new DummyAggregateRoot(id);

        Assert.NotNull(aggregate);
    }
}
