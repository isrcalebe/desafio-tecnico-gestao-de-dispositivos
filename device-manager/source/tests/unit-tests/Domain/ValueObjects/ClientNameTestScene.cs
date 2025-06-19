using DeviceManager.Domain.ValueObjects;

namespace DeviceManager.UnitTests.Domain.ValueObjects;

public class ClientNameTestScene
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_ShouldReturnError_WhenValueIsNullOrWhitespace(string input)
    {
        var result = ClientName.Create(input);

        Assert.True(result.IsFailure);
        Assert.Equal("Client name cannot be empty or whitespace.", result.Error.Message);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("a")]
    [InlineData("xyz")]
    public void Create_ShouldReturnError_WhenValueIsTooShort(string input)
    {
        var result = ClientName.Create(input);

        Assert.True(result.IsFailure);
        Assert.Equal("Client name must be at least 4 characters long.", result.Error.Message);
    }

    [Fact]
    public void Create_ShouldReturnError_WhenValueIsTooLong()
    {
        var input = new string('a', 101);
        var result = ClientName.Create(input);

        Assert.True(result.IsFailure);
        Assert.Equal("Client name must not exceed 100 characters.", result.Error.Message);
    }

    [Theory]
    [InlineData("John Doe")]
    [InlineData("Acme Corporation")]
    [InlineData("Valid Name 123")]
    public void Create_ShouldReturnClientName_WhenValueIsValid(string input)
    {
        var result = ClientName.Create(input);

        Assert.True(result.IsSuccess);
        Assert.Equal(input, result.Value.Value);
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        var name = "Test Client";
        var result = ClientName.Create(name);

        Assert.True(result.IsSuccess);
        Assert.Equal(name, result.Value.ToString());
    }

    [Fact]
    public void Equals_ShouldReturnTrue_ForSameValue()
    {
        var name = "Same Name";
        var a = ClientName.Create(name).Value;
        var b = ClientName.Create(name).Value;

        Assert.True(a.Equals(b));
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void Equals_ShouldReturnFalse_ForDifferentValues()
    {
        var a = ClientName.Create("Name A").Value;
        var b = ClientName.Create("Name B").Value;

        Assert.False(a.Equals(b));
    }

    [Fact]
    public void ImplicitOperator_ShouldReturnStringValue()
    {
        var name = "Implicit Name";
        var clientName = ClientName.Create(name).Value;

        string str = clientName;

        Assert.Equal(name, str);
    }
}
