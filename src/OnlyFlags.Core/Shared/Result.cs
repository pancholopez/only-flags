namespace OnlyFlags.Core.Shared;

//inspired by https://github.com/dotnet/csharplang/blob/main/proposals/TypeUnions.md#common-unions
public readonly struct Result
{
    public Error Error { get; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    private Result(Error error, bool isSuccess)
    {
        Error = error;
        IsSuccess = isSuccess;
    }

    public static Result Success() => new(error: default!, isSuccess: true);
    public static Result Failure(Error error) => new(error: error, isSuccess: false);
    public static implicit operator Result(Error error) => Failure(error);
}

public readonly struct Result<T>
{
    public T Value { get; private init; }
    public Error[] Errors { get; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    private Result(T value,ICollection<Error> errors, bool isSuccess)
    {
        if (errors.Count > 0 & isSuccess) 
            throw new InvalidOperationException();
        
        Value = value;
        Errors = errors.ToArray();
        IsSuccess = isSuccess;
    }

    public static Result<T> Success(T value) => new(value, [], true);
    public static Result<T> Failure(ICollection<Error> errors) => new(default!, errors, false);
    public static Result<T> Failure(Error error) => new(default!, [error], false);

    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(Error error) => Failure(error);
    public static implicit operator Result<T>(Error[] errors) => Failure(errors);
    public static implicit operator Result<T>(List<Error> errors) => Failure(errors);
}