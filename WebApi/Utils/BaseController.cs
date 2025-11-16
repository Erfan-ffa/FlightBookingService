using System.Net;
using Application.Utils;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Utils;

[ApiController]
[Route("[controller]/[action]")]
public class BaseController : ControllerBase
{
    protected IActionResult ToApiResult<T>(ApiResponse<T> value)
    {
        return value.StatusCode switch
        {
            HttpStatusCode.OK => Ok(value),
            HttpStatusCode.BadRequest => BadRequest(value),
            HttpStatusCode.NoContent => NoContent(),
            HttpStatusCode.NotFound => NotFound(value),
            HttpStatusCode.Conflict => Conflict(value),
            HttpStatusCode.Gone => StatusCode(409),
            _ => Accepted(value)
        };
    }
    
    protected IActionResult ToApiResult(ApiResponse value)
    {
        return value.StatusCode switch
        {
            HttpStatusCode.OK => Ok(value),
            HttpStatusCode.BadRequest => BadRequest(value),
            HttpStatusCode.NoContent => NoContent(),
            HttpStatusCode.NotFound => NotFound(value),
            HttpStatusCode.Conflict => Conflict(value),
            HttpStatusCode.Gone => StatusCode(409),
            _ => throw new Exception("Invalid Status Code.")
        };
    }
}