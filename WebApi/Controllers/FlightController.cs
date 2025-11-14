using Application.Features.Flights.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebApi.Controllers;

[ApiController]
[Route("[action]/[controller]")]
public class FlightController(IMediator mediator) : ControllerBase
{
    
    [HttpPost]
    [Idempotent]
    public async Task<IActionResult> Create([FromBody] CreateFlightRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        return Ok(result);
    }
}