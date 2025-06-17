using System.Diagnostics.CodeAnalysis;

namespace DeviceManager.Common;

/// <summary>
/// Represents a result that can either be a success with a value or a failure with an error.
/// This is a generic type that allows you to specify the type of the value and the type of the error.
/// </summary>
/// <typeparam name="T">
/// The type of the value in case of success.
/// </typeparam>
/// <typeparam name="E">
/// The type of the error in case of failure.
/// </typeparam>
public readonly struct Result<T, E> : IEquatable<Result<T, E>>
    where E : notnull
{
    private readonly T? value;
    private readonly E? error;

    /// <summary>
    /// Indicates whether the result is a success (true) or a failure (false).
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Indicates whether the result is a failure (true) or a success (false).
    /// </summary>
    public bool IsFailure => !IsSuccess;

    private Result(T value)
    {
        IsSuccess = true;
        this.value = value;
        error = default;
    }

    private Result(E error)
    {
        IsSuccess = false;
        value = default;
        this.error = error;
    }

    /// <summary>
    /// Creates a successful result with the specified value.
    /// </summary>
    /// <param name="value">
    /// The value to be wrapped in the result.
    /// </param>
    /// <returns>
    /// A <see cref="Result{T, E}"/> representing a successful operation with the specified value.
    /// </returns>
    public static Result<T, E> Success(T value)
        => new(value);

    /// <summary>
    /// Creates a failure result with the specified error.
    /// </summary>
    /// <param name="error">
    /// The error to be wrapped in the result.
    /// </param>
    /// <returns>
    /// A <see cref="Result{T, E}"/> representing a failed operation with the specified error.
    /// </returns>
    public static Result<T, E> Failure(E error)
        => new(error);

    /// <summary>
    /// Gets the value of the result if it is a success.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the result is a failure and the value is accessed.
    /// </exception>
    public T Value => IsSuccess
        ? value!
        : throw new InvalidOperationException("Cannot access the value of a failure result.");

    /// <summary>
    /// Gets the error of the result if it is a failure.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the result is a success and the error is accessed.
    /// </exception>
    public E Error => IsFailure
        ? error!
        : throw new InvalidOperationException("Cannot access the error of a success result.");

    /// <summary>
    /// Tries to get the value of the result if it is a success.
    /// </summary>
    /// <param name="value">
    /// The output parameter that will contain the value if the result is a success.
    /// </param>
    /// <returns>
    /// True if the result is a success and the value is set; otherwise, false.
    /// </returns>
    public bool TryGetValue([MaybeNullWhen(false)] out T value)
    {
        value = this.value;
        return IsSuccess;
    }

    /// <summary>
    /// Tries to get the error of the result if it is a failure.
    /// </summary>
    /// <param name="error">
    /// The output parameter that will contain the error if the result is a failure.
    /// </param>
    /// <returns>
    /// True if the result is a failure and the error is set; otherwise, false.
    /// </returns>
    public bool TryGetError([MaybeNullWhen(false)] out E error)
    {
        error = this.error;
        return IsFailure;
    }

    /// <summary>
    /// Executes the specified action if the result is a success, or another action if it is a failure.
    /// </summary>
    /// <param name="onSuccess">
    /// The action to execute if the result is a success.
    /// </param>
    /// <param name="onFailure">
    /// The action to execute if the result is a failure.
    /// </param>
    public void Match(Action<T> onSuccess, Action<E> onFailure)
    {
        if (IsSuccess)
            onSuccess(value!);
        else
            onFailure(error!);
    }

    /// <summary>
    /// Executes the specified functions and returns a result based on whether the result is a success or a failure.
    /// </summary>
    /// <typeparam name="TResult">
    /// The type of the result to be returned based on the success or failure.
    /// </typeparam>
    /// <param name="onSuccess">
    /// The function to execute if the result is a success, returning a value of type <typeparamref name="TResult"/>.
    /// </param>
    /// <param name="onFailure">
    /// The function to execute if the result is a failure, returning a value of type <typeparamref name="TResult"/>.
    /// </param>
    /// <returns>
    /// A value of type <typeparamref name="TResult"/> based on whether the result is a success or a failure.
    /// </returns>
    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<E, TResult> onFailure)
    {
        return IsSuccess
            ? onSuccess(value!)
            : onFailure(error!);
    }

    /// <summary>
    /// Maps the value of the result to a new type if it is a success, or returns a failure with the same error if it is a failure.
    /// This is useful for transforming the value of a successful result into another type.
    /// </summary>
    /// <typeparam name="TOut">
    /// The type of the value to be returned in case of success.
    /// </typeparam>
    /// <param name="mapFunc">
    /// The function that maps the current value to a new value of type <typeparamref name="TOut"/>.
    /// </param>
    /// <returns>
    /// A <see cref="Result{TOut, E}"/> representing a successful operation with the mapped value if the result is a success,
    /// or a failure with the same error if the result is a failure.
    /// </returns>
    public Result<TOut, E> Map<TOut>(Func<T, TOut> mapFunc)
    {
        return IsSuccess
            ? Result<TOut, E>.Success(mapFunc(value!))
            : Result<TOut, E>.Failure(error!);
    }

    /// <summary>
    /// Maps the error of the result to a new type if it is a failure, or returns a success with the same value if it is a success.
    /// This is useful for transforming the error of a failed result into another type.
    /// </summary>
    /// <typeparam name="TOutError">
    /// The type of the error to be returned in case of failure.
    /// </typeparam>
    /// <param name="mapFunc">
    /// The function that maps the current error to a new error of type <typeparamref name="TOutError"/>.
    /// </param>
    /// <returns>
    /// A <see cref="Result{T, TOutError}"/> representing a successful operation with the same value if the result is a success,
    /// </returns>
    public Result<T, TOutError> MapError<TOutError>(Func<E, TOutError> mapFunc)
        where TOutError : notnull
    {
        return IsFailure
            ? Result<T, TOutError>.Failure(mapFunc(error!))
            : Result<T, TOutError>.Success(value!);
    }

    /// <summary>
    /// Executes the specified function if the result is a success, returning a new result based on the function's output.
    /// </summary>
    /// <typeparam name="TOut">
    /// The type of the value to be returned in case of success.
    /// </typeparam>
    /// <param name="thenFunc">
    /// The function to execute if the result is a success, which returns a <see cref="Result{TOut, E}"/>.
    /// </param>
    /// <returns>
    /// A <see cref="Result{TOut, E}"/> representing a successful operation with the value returned by the function if the result is a success,
    /// or a failure with the same error if the result is a failure.
    /// </returns>
    public Result<TOut, E> Then<TOut>(Func<T, Result<TOut, E>> thenFunc)
    {
        return IsSuccess
            ? thenFunc(value!)
            : Result<TOut, E>.Failure(error!);
    }

    /// <summary>
    /// Executes the specified function if the result is a failure, returning a new result based on the function's output.
    /// </summary>
    /// <param name="orElseFunc">
    /// The function to execute if the result is a failure, which returns a <see cref="Result{T, E}"/>.
    /// </param>
    /// <returns>
    /// A <see cref="Result{T, E}"/> representing a successful operation with the same value if the result is a success,
    /// </returns>
    public Result<T, E> OrElse(Func<E, Result<T, E>> orElseFunc)
    {
        return IsFailure
            ? orElseFunc(error!)
            : this;
    }

    /// <summary>
    /// Unwraps the value of the result if it is a success, or throws an exception if it is a failure.
    /// </summary>
    /// <param name="defaultValue">
    /// The default value to return if the result is a failure.
    /// </param>
    /// <returns>
    /// The value of type <typeparamref name="T"/> if the result is a success, or the specified default value if it is a failure.
    /// </returns>
    public T UnwrapOr(T defaultValue) => IsSuccess ? value! : defaultValue;

    /// <summary>
    /// Unwraps the value of the result if it is a success, or executes a function to get a default value if it is a failure.
    /// </summary>
    /// <param name="defaultFunc">
    /// The function to execute to get a default value if the result is a failure.
    /// </param>
    /// <returns>
    /// The value of type <typeparamref name="T"/> if the result is a success, or the value returned by the function if it is a failure.
    /// </returns>
    public T UnwrapOrElse(Func<E, T> defaultFunc) => IsSuccess ? value! : defaultFunc(error!);

    public static implicit operator Result<T, E>(T value) => Success(value);
    public static implicit operator Result<T, E>(E error) => Failure(error);

    public override bool Equals(object? obj) => obj is Result<T, E> other && Equals(other);

    public bool Equals(Result<T, E> other)
    {
        if (IsSuccess != other.IsSuccess)
            return false;

        return IsSuccess
            ? EqualityComparer<T?>.Default.Equals(value, other.value)
            : EqualityComparer<E?>.Default.Equals(error, other.error);
    }

    public override int GetHashCode()
    {
        return IsSuccess
            ? HashCode.Combine(IsSuccess, value)
            : HashCode.Combine(IsSuccess, error);
    }

    public static bool operator ==(Result<T, E> left, Result<T, E> right) => left.Equals(right);
    public static bool operator !=(Result<T, E> left, Result<T, E> right) => !(left == right);

    public override string ToString()
    {
        return IsSuccess
            ? $"Success({value?.ToString() ?? "null"})"
            : $"Failure({error?.ToString() ?? "null"})";
    }
}

/// <summary>
/// Provides static methods to create success and failure results.
/// </summary>
public static class Result
{
    /// <summary>
    /// Creates a successful result with the specified value.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the value in case of success.
    /// </typeparam>
    /// <typeparam name="E">
    /// The type of the error in case of failure.
    /// </typeparam>
    /// <param name="value">
    /// The value to be wrapped in the result.
    /// </param>
    /// <returns>
    /// A <see cref="Result{T, E}"/> representing a successful operation with the specified value.
    /// </returns>
    public static Result<T, E> Success<T, E>(T value)
        where E : notnull
        => Result<T, E>.Success(value);

    /// <summary>
    /// Creates a failure result with the specified error.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the value in case of success (not used in this case).
    /// </typeparam>
    /// <typeparam name="E">
    /// The type of the error in case of failure.
    /// </typeparam>
    /// <param name="error">
    /// The error to be wrapped in the result.
    /// </param>
    /// <returns>
    /// A <see cref="Result{T, E}"/> representing a failed operation with the specified error.
    /// </returns>
    public static Result<T, E> Failure<T, E>(E error)
        where E : notnull
        => Result<T, E>.Failure(error);
}
