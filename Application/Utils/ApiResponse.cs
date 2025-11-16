using System.Net;
using System.Text.Json.Serialization;

namespace Application.Utils;

public class BaseApiResponse
{
    [JsonIgnore]
    public HttpStatusCode StatusCode { get; set; }
    public string Message { get; set; }
}

public class ApiResponse<T> : BaseApiResponse
{
    public T Data { get; set; }

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

public class ApiResponse : BaseApiResponse
{
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