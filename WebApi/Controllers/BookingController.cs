using Application.Features.Bookings.Book;
using Application.Features.Bookings.Query.ListByFlightId;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Utils;

namespace WebApi.Controllers;

public class BookingController(IMediator mediator) : BaseController
{
    [HttpGet("{flightId:long}")]
    public async Task<IActionResult> Get(long flightId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetBookingsByFlightIdRequest(flightId), cancellationToken);
        return ToApiResult(result);
    }

    [HttpPost]
    [Idempotent]
    public async Task<IActionResult> Book([FromBody] CreateBookingRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        return ToApiResult(result);
    }
}
