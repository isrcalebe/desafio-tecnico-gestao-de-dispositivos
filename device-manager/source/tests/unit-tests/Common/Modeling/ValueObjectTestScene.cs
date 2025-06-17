using DeviceManager.Common.Modeling;

namespace DeviceManager.UnitTests.Common.Modeling;

public class ValueObjectTest
{
    public class Money : ValueObject
    {
        public decimal Amount { get; }
        public string Currency { get; }

        public Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }
    }

    [Fact]
    public void Equals_ReturnsTrue_ForSameValues()
    {
        var a = new Money(10, "USD");
        var b = new Money(10, "USD");

        Assert.True(a.Equals(b));
        Assert.True(a == b);
        Assert.False(a != b);
    }

    [Fact]
    public void Equals_ReturnsFalse_ForDifferentValues()
    {
        var a = new Money(10, "USD");
        var b = new Money(20, "USD");
        var c = new Money(10, "EUR");

        Assert.False(a.Equals(b));
        Assert.False(a == b);
        Assert.True(a != b);

        Assert.False(a.Equals(c));
        Assert.False(a == c);
        Assert.True(a != c);
    }

    [Fact]
    public void Equals_ReturnsFalse_ForNull()
    {
        var a = new Money(10, "USD");
        Money? b = null;

        Assert.False(a.Equals(b));
        Assert.False(a == b);
        Assert.True(a != b);
    }

    [Fact]
    public void Equals_ReturnsTrue_ForBothNull()
    {
        Money? a = null;
        Money? b = null;

        Assert.True(a == b);
        Assert.False(a != b);
    }

    [Fact]
    public void GetHashCode_SameForEqualObjects()
    {
        var a = new Money(10, "USD");
        var b = new Money(10, "USD");

        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void GetHashCode_DifferentForDifferentObjects()
    {
        var a = new Money(10, "USD");
        var b = new Money(20, "USD");

        Assert.NotEqual(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void Equals_ReturnsFalse_ForDifferentTypes()
    {
        var a = new Money(10, "USD");
        var b = new DummyValueObject(10);

        Assert.False(a.Equals(b));
    }

    private class DummyValueObject : ValueObject
    {
        private readonly int value;

        public DummyValueObject(int value) => this.value = value;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return value;
        }
    }
}
