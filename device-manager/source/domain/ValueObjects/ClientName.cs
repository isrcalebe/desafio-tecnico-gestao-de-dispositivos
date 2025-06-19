using DeviceManager.Common;
using DeviceManager.Common.Modeling;

namespace DeviceManager.Domain.ValueObjects;

public class ClientName : ValueObject
{
    public string Value { get; private set; }

    private ClientName(string value)
    {
        Value = value;
    }

    public static Result<ClientName, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return new Error("Client name cannot be empty or whitespace.");

        if (value.Length < 4)
            return new Error("Client name must be at least 4 characters long.");

        if (value.Length > 100)
            return new Error("Client name must not exceed 100 characters.");

        return new ClientName(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public override bool Equals(object? obj) => obj is ClientName other && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public static implicit operator string(ClientName clientName) => clientName.Value;
}
