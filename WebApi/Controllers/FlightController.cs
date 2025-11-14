using Application.Features.Flights.Commands.Create;
using Application.Features.Flights.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebApi.Controllers;

[ApiController]
[Route("[action]/[controller]")]
public class FlightController(IMediator mediator) : ControllerBase
{
    
    [HttpPost]
    // [Idempotent]
    public async Task<IActionResult> Create([FromBody] CreateFlightRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Get([FromBody] GetFlightsQuery getFlightsQuery, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(getFlightsQuery, cancellationToken);
        return Ok(result);
    }
}