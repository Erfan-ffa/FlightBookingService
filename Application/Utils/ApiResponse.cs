using System.Net;

namespace Application.Utils;

public class ApiResponse<T>
{
    public T Data { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public string Message { get; set; }

    public static ApiResponse<T> Ok(T data)
    {
        return new ApiResponse<T> { Data = data, StatusCode = HttpStatusCode.OK };
    }

    public static ApiResponse<T> Error(string message, HttpStatusCode statusCode)
    {
        return new ApiResponse<T> { Message = message, StatusCode = statusCode };
    }
    
    public static ApiResponse<T> BadRequest(string message)
    {
        return new ApiResponse<T> { Message = message, StatusCode = HttpStatusCode.BadRequest };
    }
}

public class ApiResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public string Message { get; set; }

    public static ApiResponse Ok()
    {
        return new ApiResponse{StatusCode = HttpStatusCode.OK };
    }
    
    public static ApiResponse BadRequest(string message)
    {
        return new ApiResponse { Message = message, StatusCode = HttpStatusCode.BadRequest };
    }

    public static ApiResponse Error(string message, HttpStatusCode statusCode)
    {
        return new ApiResponse { Message = message, StatusCode = statusCode };
    }
}