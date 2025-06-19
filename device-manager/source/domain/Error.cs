namespace DeviceManager.Domain;

public record Error
{
    public string Message { get; init; } = string.Empty;

    public IReadOnlyList<string> AdditionalInfo { get; init; }

    public Error(string message, params string[] additionalInfo)
    {
        Message = message;
        AdditionalInfo = additionalInfo;
    }

    public override string ToString() => Message;
}
