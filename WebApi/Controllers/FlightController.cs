using Application.Features.Flights.Commands.Create;
using Application.Features.Flights.Commands.Update;
using Application.Features.Flights.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Utils;

namespace WebApi.Controllers;

public class FlightController(IMediator mediator) : BaseController
{
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFlightRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        return ToApiResult(result);
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetFlightsQuery getFlightsQuery, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(getFlightsQuery, cancellationToken);
        return ToApiResult(result);
    }

    [HttpPatch("Available-Seats")]
    public async Task<IActionResult> UpdateAvailableSeats([FromBody] UpdateAvailableSeatsRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        return ToApiResult(result);
    }
}