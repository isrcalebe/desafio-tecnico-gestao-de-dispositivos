namespace DeviceManager.Domain;

public record Error
{
    public string Message { get; init; } = string.Empty;

    public IReadOnlyList<string> AdditionalInfo { get; init; } = [];

    public Error(string message, IReadOnlyList<string>? additionalInfo = null)
    {
        Message = message;
        AdditionalInfo = additionalInfo ?? [];
    }

    public static Error WithMessage(string message)
        => new(message);

    public override string ToString() => Message;
}

public static class ErrExtensions
{
    public static Error WithAdditionalInfo(this Error err, params string[] additionalInfo)
        => new(err.Message, [.. err.AdditionalInfo, .. additionalInfo]);
}
