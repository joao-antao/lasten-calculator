namespace Lasten.Application;

/// <summary>
/// Represents the outcome of an operation that can either succeed with a value or fail with an error.
/// Use for expected business errors; use exceptions for unexpected system failures.
/// </summary>
public readonly record struct Result<TValue, TError>
{
    private readonly TValue? _value;
    private readonly TError? _error;
    private readonly bool _isSuccess;

    private Result(TValue value)
    {
        _value = value;
        _error = default;
        _isSuccess = true;
    }

    private Result(TError error)
    {
        _value = default;
        _error = error;
        _isSuccess = false;
    }

    public bool IsSuccess => _isSuccess;
    public bool IsFailure => !_isSuccess;

    public TValue Value => _isSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access Value of a failed result.");

    public TError Error => !_isSuccess
        ? _error!
        : throw new InvalidOperationException("Cannot access Error of a successful result.");

    public static Result<TValue, TError> Success(TValue value) => new(value);
    public static Result<TValue, TError> Failure(TError error) => new(error);

    public Result<TOut, TError> Map<TOut>(Func<TValue, TOut> mapper) =>
        _isSuccess
            ? Result<TOut, TError>.Success(mapper(_value!))
            : Result<TOut, TError>.Failure(_error!);

    public Result<TOut, TError> Bind<TOut>(Func<TValue, Result<TOut, TError>> binder) =>
        _isSuccess ? binder(_value!) : Result<TOut, TError>.Failure(_error!);

    public TValue GetValueOr(TValue defaultValue) =>
        _isSuccess ? _value! : defaultValue;

    public TResult Match<TResult>(Func<TValue, TResult> onSuccess, Func<TError, TResult> onFailure) =>
        _isSuccess ? onSuccess(_value!) : onFailure(_error!);
}
