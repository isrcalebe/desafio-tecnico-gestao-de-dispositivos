using System.Text.RegularExpressions;
using DeviceManager.Common;
using DeviceManager.Common.Modeling;

namespace DeviceManager.Domain.ValueObjects;

public partial class Email : ValueObject
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email, Error> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return new Error("Email cannot be empty or whitespace.");

        if (!emailRegex().IsMatch(email))
            return new Error("Email format is invalid.", "It should be in the format 'example@domain.com'.");

        return new Email(email.ToLowerInvariant());
    }

    public override string ToString() => Value;

    public override bool Equals(object? obj) => obj is Email other && Value == other.Value;

    public override int GetHashCode()
        => Value.GetHashCode();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(Email email) => email.Value;

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex emailRegex();
}
