using DeviceManager.Common.Modeling;

namespace DeviceManager.UnitTests.Common.Modeling;

public class EntityTestScene
{
    public class TestEntity : Entity
    {
        public TestEntity(Guid id) : base(id) { }

        public TestEntity() : base() { }

        public void SetId(Guid id)
            => Id = id;

        public void SetCreatedAt(DateTime dt)
            => CreatedAt = dt;

        public void SetLastUpdatedAt(DateTime dt)
            => LastUpdatedAt = dt;
    }

    [Fact]
    public void Entities_WithSameId_AreEqual()
    {
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);

        Assert.True(entity1.Equals(entity2));
        Assert.True(entity1 == entity2);
        Assert.False(entity1 != entity2);
        Assert.Equal(entity1.GetHashCode(), entity2.GetHashCode());
    }

    [Fact]
    public void Entities_WithDifferentIds_AreNotEqual()
    {
        var entity1 = new TestEntity(Guid.NewGuid());
        var entity2 = new TestEntity(Guid.NewGuid());

        Assert.False(entity1.Equals(entity2));
        Assert.False(entity1 == entity2);
        Assert.True(entity1 != entity2);
        Assert.NotEqual(entity1.GetHashCode(), entity2.GetHashCode());
    }

    [Fact]
    public void Entity_Equals_ReturnsFalse_WhenOtherIsNull()
    {
        var entity = new TestEntity(Guid.NewGuid());

        Assert.False(entity.Equals(null));
        Assert.NotNull(entity);
    }

    [Fact]
    public void Entity_Equals_ReturnsTrue_WhenReferenceEquals()
    {
        var id = Guid.NewGuid();

        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);

        Assert.True(entity1.Equals(entity2));
        Assert.True(entity1 == entity2);
        Assert.False(entity1 != entity2);
    }

    [Fact]
    public void BothNullEntities_AreEqual()
    {
        TestEntity? entity1 = null;
        TestEntity? entity2 = null;

        Assert.True(entity1 == entity2);
        Assert.False(entity1 != entity2);
    }

    [Fact]
    public void OneNullEntity_IsNotEqual()
    {
        var entity = new TestEntity(Guid.NewGuid());
        TestEntity? nullEntity = null;

        Assert.False(entity == nullEntity);
        Assert.True(entity != nullEntity);
        Assert.False(nullEntity == entity);
        Assert.True(nullEntity != entity);
    }

    [Fact]
    public void GetHashCode_ConsistentWithEquals()
    {
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);

        Assert.Equal(entity1, entity2);
        Assert.Equal(entity1.GetHashCode(), entity2.GetHashCode());
    }

    [Fact]
    public void Properties_AreSettableAndGettable()
    {
        var entity = new TestEntity();
        var id = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;
        var updatedAt = createdAt.AddMinutes(5);

        entity.SetId(id);
        entity.SetCreatedAt(createdAt);
        entity.SetLastUpdatedAt(updatedAt);

        Assert.Equal(id, entity.Id);
        Assert.Equal(createdAt, entity.CreatedAt);
        Assert.Equal(updatedAt, entity.LastUpdatedAt);
    }
}
