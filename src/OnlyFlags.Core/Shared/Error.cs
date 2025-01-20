namespace OnlyFlags.Core.Shared;

public record Error
{
    public required string Message { get; init; }

    public string Code { get; init; } = nameof(Error);

    public Exception? Exception { get; init; }

    private Error()
    {
    }

    public Error(string message, string code = nameof(Error), Exception? exception = null)
    {
        Message = message;
        Code = code;
        Exception = exception;
    }

    public static Error Create(Exception exception) => new()
    {
        Message = exception.Message,
        Code = exception.GetType().Name,
        Exception = exception
    };
}

internal static class ErrorExtensions
{
    public static string CombineErrorMessages(this IEnumerable<Error> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);
        return string.Join("; ", errors.Select(e => e.Message));
    }
}