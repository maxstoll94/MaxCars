namespace Helpers
{
    /// <summary>
    /// This class provides functionality for implementing Railway Oriented Programming (https://fsharpforfunandprofit.com/rop/) in C#.
    /// The following code was copied from: https://chtenb.dev/?page=rop-cs-4
    /// </summary>
    public abstract class Result<TSuccess, TFailure>
    {

        public abstract Result<TNextSuccess, TFailure> OnSuccess<TNextSuccess>(
          Func<TSuccess, Result<TNextSuccess, TFailure>> onSuccess);

        public abstract Result<TSuccess, TNextFailure> OnFailure<TNextFailure>(
          Func<TFailure, Result<TSuccess, TNextFailure>> onFailure);
        /// <summary>
        /// Prefer <see cref="OnSuccess"> and <see cref="OnFailure"> over this method for returning Result types.
        /// </summary>
        public abstract TReturn Handle<TReturn>(Func<TSuccess, TReturn> onSuccess, Func<TFailure, TReturn> onFailure);

        public bool IsSuccess() => this is Success;
        public bool IsFailure() => this is Failure;

        #region Void overloads (Because void is not an actual type in .NET)

        public void HandleVoid(Action<TSuccess> onSuccess, Action<TFailure> onFailure)
        {
            _ = onSuccess ?? throw new ArgumentNullException(nameof(onSuccess));
            _ = onFailure ?? throw new ArgumentNullException(nameof(onFailure));

            _ = Handle(onSuccess.AsFunc(), onFailure.AsFunc());
        }

        #endregion

        public sealed class Success : Result<TSuccess, TFailure>
        {
            public TSuccess ResultValue { get; }

            public Success(TSuccess result) => ResultValue = result;

            public override Result<TNextSuccess, TFailure> OnSuccess<TNextSuccess>(
              Func<TSuccess, Result<TNextSuccess, TFailure>> onSuccess)
                => onSuccess(ResultValue);

            public override Result<TSuccess, TNextFailure> OnFailure<TNextFailure>(
              Func<TFailure, Result<TSuccess, TNextFailure>> onFailure)
                => Result.Succeed(ResultValue);

            public override TReturn Handle<TReturn>(Func<TSuccess, TReturn> onSuccess, Func<TFailure, TReturn> onFailure)
                => onSuccess(ResultValue);
        }

        public sealed class Failure : Result<TSuccess, TFailure>
        {
            public TFailure ErrorValue { get; }

            public Failure(TFailure error) => ErrorValue = error;

            public override Result<TNextSuccess, TFailure> OnSuccess<TNextSuccess>(
              Func<TSuccess, Result<TNextSuccess, TFailure>> onSuccess)
                => Result.Fail(ErrorValue);

            public override Result<TSuccess, TNextFailure> OnFailure<TNextFailure>(
              Func<TFailure, Result<TSuccess, TNextFailure>> onFailure)
                => onFailure(ErrorValue);

            public override TReturn Handle<TReturn>(Func<TSuccess, TReturn> onSuccess, Func<TFailure, TReturn> onFailure)
                => onFailure(ErrorValue);
        }

        public static implicit operator Result<TSuccess, TFailure>(Result.GenericSuccess<TSuccess> success)
            => new Success(success.Value);

        public static implicit operator Result<TSuccess, TFailure>(Result.GenericFailure<TFailure> failure)
            => new Failure(failure.Value);

        private Result() { }
    }

    /// <summary>
    /// This factory method class makes it possible to create result objects without
    /// having to specify the full result type explicitly.
    /// If the generic types cannot be inferred they can also be explicitly passed.
    /// </summary>
    public static class Result
    {
        public static GenericSuccess<TSuccess> Succeed<TSuccess>(TSuccess result)
            => new(result);

        public static GenericFailure<TFailure> Fail<TFailure>(TFailure error)
            => new(error);

        public static Result<TSuccess, TFailure> Succeed<TSuccess, TFailure>(TSuccess result)
            => Succeed(result);

        public static Result<TSuccess, TFailure> Fail<TSuccess, TFailure>(TFailure error)
            => Fail(error);

        public static GenericSuccess<Unit> Succeed()
            => Succeed(Unit.unit);

        public static GenericFailure<Unit> Fail()
            => Fail(Unit.unit);

        public readonly struct GenericFailure<T>
        {
            public T Value { get; }

            public GenericFailure(T value)
            {
                Value = value;
            }
        }

        public readonly struct GenericSuccess<T>
        {
            public T Value { get; }

            public GenericSuccess(T value)
            {
                Value = value;
            }
        }
    }

    // Taken from: https://chtenb.dev/?page=unit-cs
    public struct Unit : IEquatable<Unit>
    {
        public static readonly Unit unit;
        public override bool Equals(object obj) => obj is Unit;
        public override int GetHashCode() => 0;
        public static bool operator ==(Unit left, Unit right) => left.Equals(right);
        public static bool operator !=(Unit left, Unit right) => !(left == right);
        public bool Equals(Unit other) => true;
        public override string ToString() => "()";
    }

    // Taken from: https://chtenb.dev/?page=unit-cs
    public static class ExtensionMethods
    {
        public static async Task<Unit> AsUnitTask(this Task task)
        {
            await task;
            return Unit.unit;
        }

        public static Func<TResult, Unit> AsFunc<TResult>(this Action<TResult> action)
        {
            return result =>
            {
                action(result);
                return Unit.unit;
            };
        }

        public static Func<Unit, Unit> AsFunc(this Action action)
        {
            return _ =>
            {
                action();
                return Unit.unit;
            };
        }
    }
}