using System.Net;

namespace Application.Utils;

public class ApiResponse<T>
{
    public T Data { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public string Code { get; set; }

    public static ApiResponse<T> Ok(T data)
    {
        return new ApiResponse<T> { Data = data, StatusCode = HttpStatusCode.OK };
    }

    public static ApiResponse<T> Error(string code, HttpStatusCode statusCode)
    {
        return new ApiResponse<T> { Code = code, StatusCode = statusCode };
    }
}

public class ApiResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public string Code { get; set; }

    public static ApiResponse Ok()
    {
        return new ApiResponse{StatusCode = HttpStatusCode.OK };
    }

    public static ApiResponse Error(string code, HttpStatusCode statusCode)
    {
        return new ApiResponse { Code = code, StatusCode = statusCode };
    }
}