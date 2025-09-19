namespace Domain.Results;

public class Result<TValue> where TValue : class
{
    public string[]? Errors { get; set; }
    public bool IsSuccess { get; set; }
    public TValue? Value { get; set; }

    public static Result<TValue> Failure(string error) =>
        new() {
            Errors = [error],
            IsSuccess = false,
            Value = null
        };
    
    public static Result<TValue> Failure(string[] errors) =>
        new() {
            Errors = errors,
            IsSuccess = false,
            Value = null
        };

    public static Result<TValue> Success(TValue entity) =>
        new()
        {
            IsSuccess = true,
            Errors = null,
            Value = entity
        };
    public static Result<TValue> PartialFailure(string[] errors, TValue value) => new() {Errors = errors, Value = value};
}

public class ResultValue<TValue> where TValue : struct
{
    public string[]? Errors { get; set; }
    public bool IsSuccess { get; set; }
    public TValue Value { get; set; }

    public static ResultValue<TValue> Failure(string error) =>
        new() {
            Errors = [error],
            IsSuccess = false,
            Value = default
        };
    
    public static ResultValue<TValue> Failure(string[] errors) =>
        new() {
            Errors = errors,
            IsSuccess = false,
            Value = default
        };

    public static ResultValue<TValue> Success(TValue entity) =>
        new()
        {
            IsSuccess = true,
            Errors = null,
            Value = entity
        };
    public static ResultValue<TValue> PartialFailure(string[] errors, TValue value) => new() {Errors = errors, Value = value};

}

public class Result
{
    public string[]? Errors { get; set; }
    public bool IsSuccess { get; set; }
    
    public static Result Failure(string error) => new Result(){ Errors = [error], IsSuccess = false };
    public static Result Failure(string[] errors) => new Result(){ Errors = errors, IsSuccess = false };
    public static Result Success() => new Result(){ IsSuccess = true };
}