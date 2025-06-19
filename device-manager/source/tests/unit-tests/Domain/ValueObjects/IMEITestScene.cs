using DeviceManager.Domain.ValueObjects;

namespace DeviceManager.UnitTests.Domain.ValueObjects;

public class IMEITestScene
{
    [Fact]
    public void Create_WithValid15DigitIMEI_ReturnsSuccess()
    {
        var validImei = "123456789012345";

        var result = IMEI.Create(validImei);

        Assert.True(result.IsSuccess);
        Assert.Equal(validImei, result.Value.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_WithNullOrWhitespace_ReturnsError(string? input)
    {
        var result = IMEI.Create(input);

        Assert.True(result.IsFailure);
        Assert.Equal("IMEI cannot be empty or whitespace.", result.Error.Message);
    }

    [Theory]
    [InlineData("12345678901234")]   // 14 digits
    [InlineData("1234567890123456")] // 16 digits
    [InlineData("ABCDEFGHIJKLMNO")]  // Non-numeric
    [InlineData("12345abc9012345")]  // Mixed
    public void Create_WithInvalidFormat_ReturnsError(string input)
    {
        var result = IMEI.Create(input);

        Assert.True(result.IsFailure);
        Assert.Equal("IMEI format is invalid.", result.Error.Message);
        Assert.Equal("It should be a 15-digit number.", result.Error.AdditionalInfo[0]);
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        var imei = IMEI.Create("123456789012345").Value;
        Assert.Equal("123456789012345", imei.ToString());
    }

    [Fact]
    public void Equals_And_GetHashCode_WorkCorrectly()
    {
        var imei1 = IMEI.Create("123456789012345").Value;
        var imei2 = IMEI.Create("123456789012345").Value;
        var imei3 = IMEI.Create("543210987654321").Value;

        Assert.True(imei1.Equals(imei2));
        Assert.False(imei1.Equals(imei3));
        Assert.Equal(imei1.GetHashCode(), imei2.GetHashCode());
        Assert.NotEqual(imei1.GetHashCode(), imei3.GetHashCode());
    }

    [Fact]
    public void ImplicitOperatorString_ReturnsValue()
    {
        var imei = IMEI.Create("123456789012345").Value;
        string value = imei;

        Assert.Equal("123456789012345", value);
    }
}
