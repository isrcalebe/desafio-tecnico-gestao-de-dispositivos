using DeviceManager.Common;
using DeviceManager.Common.Modeling;

namespace DeviceManager.Domain.ValueObjects;

public class PhoneNumber : ValueObject
{
    public string Value { get; private set; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static Result<PhoneNumber, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return new Error("Phone number cannot be empty or whitespace.");

        if (value.Length < 10 || value.Length > 15)
            return new Error("Phone number must be between 10 and 15 characters long.");

        if (!value.All(char.IsDigit))
        {
            return new Error("Phone number must contain only digits.", "No spaces, dashes, or other characters are allowed.");
        }

        return new PhoneNumber(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public override bool Equals(object? obj) => obj is PhoneNumber other && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public static implicit operator string?(PhoneNumber? phoneNumber) => phoneNumber?.Value;
}
