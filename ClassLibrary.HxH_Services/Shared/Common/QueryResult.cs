namespace ClassLibrary.HxH_Services.Shared.Common;

public class QueryResult<T>
{
    public bool IsSuccess { get; set; }
    public bool IsFailure => !IsSuccess;
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    private QueryResult(bool isSuccess, string message, T data = default)
    {
        IsSuccess = isSuccess;
        Message = message;
        Data = data;
    }

    public static QueryResult<T> Success(string message = "Ok", T? data = default) => new(true, message, data);
    public static QueryResult<T> Failure(string message = "Error") => new(false, message);
}
