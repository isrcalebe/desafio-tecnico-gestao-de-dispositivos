using System.Text.RegularExpressions;
using DeviceManager.Common;
using DeviceManager.Common.Modeling;

namespace DeviceManager.Domain.ValueObjects;

public partial class IMEI : ValueObject
{
    public string Value { get; private set; }

    private IMEI(string value)
    {
        Value = value;
    }

    public static Result<IMEI, Error> Create(string imei)
    {
        if (string.IsNullOrWhiteSpace(imei))
            return Error.WithMessage("IMEI cannot be empty or whitespace.");

        if (!imeiRegex().IsMatch(imei))
        {
            return Error.WithMessage("IMEI format is invalid.")
                        .WithAdditionalInfo("It should be a 15-digit number.");
        }

        return new IMEI(imei);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public override bool Equals(object? obj) => obj is IMEI other && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public static implicit operator string(IMEI imei) => imei.Value;

    [GeneratedRegex(@"^\d{15}$")]
    private static partial Regex imeiRegex();
}
