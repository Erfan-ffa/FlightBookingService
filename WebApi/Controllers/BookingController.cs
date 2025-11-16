using Application.Features.Bookings.Book;
using Application.Features.Bookings.Query.ListByFlightId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[action]/[controller]")]
public class BookingController(IMediator mediator) : ControllerBase
{
    [HttpGet("{flightId:long}")]
    public async Task<IActionResult> Get(long flightId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetBookingsByFlightIdRequest(flightId), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Book([FromBody] CreateBookingRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        return Ok(result);
    }
}