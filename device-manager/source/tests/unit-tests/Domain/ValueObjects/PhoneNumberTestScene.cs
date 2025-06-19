using DeviceManager.Domain.ValueObjects;

namespace DeviceManager.UnitTests.Domain.ValueObjects;

public class PhoneNumberTestScene
{
    [Theory]
    [InlineData("1234567890")]
    [InlineData("012345678901234")]
    public void Create_ValidPhoneNumber_ReturnsSuccess(string validNumber)
    {
        var result = PhoneNumber.Create(validNumber);

        Assert.True(result.IsSuccess);
        Assert.Equal(validNumber, result.Value.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_EmptyOrWhitespace_ReturnsError(string invalidNumber)
    {
        var result = PhoneNumber.Create(invalidNumber);

        Assert.True(result.IsFailure);
        Assert.Equal("Phone number cannot be empty or whitespace.", result.Error.Message);
    }

    [Theory]
    [InlineData("123456789")] // 9 digits
    [InlineData("1234567890123456")] // 16 digits
    public void Create_InvalidLength_ReturnsError(string invalidNumber)
    {
        var result = PhoneNumber.Create(invalidNumber);

        Assert.True(result.IsFailure);
        Assert.Equal("Phone number must be between 10 and 15 characters long.", result.Error.Message);
    }

    [Theory]
    [InlineData("123-456-7890")]
    [InlineData("123 456 7890")]
    [InlineData("123456789O")] // Letter O instead of zero
    [InlineData("123456789!")]
    public void Create_NonDigitCharacters_ReturnsError(string invalidNumber)
    {
        var result = PhoneNumber.Create(invalidNumber);

        Assert.True(result.IsFailure);
        Assert.Equal("Phone number must contain only digits.", result.Error.Message);
        Assert.Equal("No spaces, dashes, or other characters are allowed.", result.Error.AdditionalInfo[0]);
    }

    [Fact]
    public void ToString_ReturnsPhoneNumberValue()
    {
        var result = PhoneNumber.Create("1234567890");

        Assert.True(result.IsSuccess);
        Assert.Equal("1234567890", result.Value.ToString());
    }

    [Fact]
    public void Equals_SameValue_ReturnsTrue()
    {
        var result1 = PhoneNumber.Create("1234567890");
        var result2 = PhoneNumber.Create("1234567890");

        Assert.True(result1.IsSuccess && result2.IsSuccess);
        Assert.True(result1.Value.Equals(result2.Value));
    }

    [Fact]
    public void Equals_DifferentValue_ReturnsFalse()
    {
        var result1 = PhoneNumber.Create("1234567890");
        var result2 = PhoneNumber.Create("0987654321");

        Assert.True(result1.IsSuccess && result2.IsSuccess);
        Assert.False(result1.Value.Equals(result2.Value));
    }

    [Fact]
    public void ImplicitOperator_ReturnsStringValue()
    {
        var result = PhoneNumber.Create("1234567890");
        Assert.True(result.IsSuccess);

        string phone = result.Value;
        Assert.Equal("1234567890", phone);
    }
}
