using DeviceManager.Domain.ValueObjects;

namespace DeviceManager.UnitTests.Domain.ValueObjects;

public class SerialNumberTestScene
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_ShouldReturnError_WhenSerialNumberIsNullOrWhitespace(string serialNumber)
    {
        var result = SerialNumber.Create(serialNumber);

        Assert.True(result.IsFailure);
        Assert.Equal("Serial number cannot be empty or whitespace.", result.Error.Message);
    }

    [Fact]
    public void Create_ShouldReturnError_WhenSerialNumberFormatIsInvalid()
    {
        var result = SerialNumber.Create("INVALID-SERIAL");

        Assert.True(result.IsFailure);
        Assert.Equal("Serial number format is invalid.", result.Error.Message);
        Assert.Contains("It should be in the format", result.Error.AdditionalInfo[0]);
    }

    [Fact]
    public void Create_ShouldReturnSerialNumber_WhenFormatIsValid()
    {
        var validSerial = "SN-2024-ABC-1A2B3C4D";
        var result = SerialNumber.Create(validSerial);

        Assert.True(result.IsSuccess);
        Assert.Equal(validSerial, result.Value.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateManufacturer_ShouldReturnError_WhenManufacturerCodeIsNullOrWhitespace(string manufacturerCode)
    {
        var result = SerialNumber.CreateManufacturer(manufacturerCode);

        Assert.True(result.IsFailure);
        Assert.Equal("Manufacturer code cannot be empty or whitespace.", result.Error.Message);
    }

    [Theory]
    [InlineData("AB")]
    [InlineData("abcd")]
    [InlineData("12A")]
    [InlineData("aBc")]
    public void CreateManufacturer_ShouldReturnError_WhenManufacturerCodeIsInvalid(string code)
    {
        var result = SerialNumber.CreateManufacturer(code);

        Assert.True(result.IsFailure);
        Assert.Equal("Manufacturer code must be exactly 3 uppercase letters.", result.Error.Message);
    }

    [Fact]
    public void CreateManufacturer_ShouldReturnSerialNumber_WhenManufacturerCodeIsValid()
    {
        var code = "XYZ";
        var result = SerialNumber.CreateManufacturer(code);

        Assert.True(result.IsSuccess);
        Assert.StartsWith($"SN-{DateTime.UtcNow.Year}-XYZ-", result.Value.Value);
        Assert.Equal(20, result.Value.Value.Length); // SN-YYYY-XXX-XXXXXXXX
    }

    [Fact]
    public void SerialNumber_Equality_ShouldWork()
    {
        var serial = "SN-2024-ABC-1A2B3C4D";
        var sn1 = SerialNumber.Create(serial).Value;
        var sn2 = SerialNumber.Create(serial).Value;

        Assert.Equal(sn1, sn2);
        Assert.True(sn1.Equals(sn2));
        Assert.Equal(sn1.GetHashCode(), sn2.GetHashCode());
    }

    [Fact]
    public void SerialNumber_ToString_ReturnsValue()
    {
        var serial = "SN-2024-ABC-1A2B3C4D";
        var sn = SerialNumber.Create(serial).Value;

        Assert.Equal(serial, sn.ToString());
    }

    [Fact]
    public void SerialNumber_ImplicitOperator_ReturnsValue()
    {
        var serial = "SN-2024-ABC-1A2B3C4D";
        var sn = SerialNumber.Create(serial).Value;

        string value = sn;
        Assert.Equal(serial, value);
    }
}
