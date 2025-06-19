using System.Text.RegularExpressions;
using DeviceManager.Common;
using DeviceManager.Common.Modeling;

namespace DeviceManager.Domain.ValueObjects;

public partial class SerialNumber : ValueObject
{
    public string Value { get; private set; }

    private SerialNumber(string value)
    {
        Value = value;
    }

    public static Result<SerialNumber, Error> Create(string serialNumber)
    {
        if (string.IsNullOrWhiteSpace(serialNumber))
            return new Error("Serial number cannot be empty or whitespace.");

        if (!serialRegex().IsMatch(serialNumber))
        {
            return new Error("Serial number format is invalid.",
                             "It should be in the format 'SN-YYYY-MMM-XXXXXXXX'.",
                            "Where YYYY is a 4-digit year, MMM is a 3-letter manufacturer code, and XXXXXXXXXX is an 8-character alphanumeric code.");
        }

        return new SerialNumber(serialNumber);
    }

    public static Result<SerialNumber, Error> CreateManufacturer(string manufacturerCode)
    {
        if (string.IsNullOrWhiteSpace(manufacturerCode))
            return new Error("Manufacturer code cannot be empty or whitespace.");

        if (manufacturerCode.Length != 3 || !manufacturerRegex().IsMatch(manufacturerCode))
            return new Error("Manufacturer code must be exactly 3 uppercase letters.");

        var year = DateTime.UtcNow.Year;
        var guid = Guid.CreateVersion7();
        var suffix = guid.ToString("N")[..8].ToUpperInvariant();

        var serial = $"SN-{year}-{manufacturerCode.ToUpperInvariant()}-{suffix}";

        return new SerialNumber(serial);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public override bool Equals(object? obj) => obj is SerialNumber other && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public static implicit operator string(SerialNumber serialNumber) => serialNumber.Value;

    [GeneratedRegex(@"^SN-\d{4}-[A-Z]{3}-[A-Z0-9]{8}$", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex serialRegex();

    [GeneratedRegex(@"^[A-Z]{3}$")]
    private static partial Regex manufacturerRegex();
}
