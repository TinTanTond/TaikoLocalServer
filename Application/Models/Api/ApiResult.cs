namespace Application.Models.Api;


public class ApiResult
{
    public bool Succeeded { get; init; }
    public string? Message { get; init; }
    
    public static ApiResult Failed(string message) => new()
    {
        Succeeded = false,
        Message = message
    };
    
    public static ApiResult<T> Failed<T>(string message) => new()
    {
        Succeeded = false,
        Message = message
    };
    
    public static ApiResult<T> Succeed<T>(T data) => new()
    {
        Succeeded = true,
        Data = data
    };
}

public class ApiResult<T> : ApiResult
{
    public T? Data { get; init; }
}