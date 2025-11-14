using Application.Contracts.Repositories;
using Application.Features.Bookings.Models;
using Application.Utils;
using MediatR;

namespace Application.Features.Bookings.Query.ListByFlightId;

public class GetBookingsByFlightIdHandler(IUnitOfWork uow) 
    : IRequestHandler<GetBookingsByFlightIdRequest, ApiResponse<GetBookingsByFlightIdResponse>>
{
    public async Task<ApiResponse<GetBookingsByFlightIdResponse>> Handle(GetBookingsByFlightIdRequest request, CancellationToken cancellationToken)
    {
        var flightBookings = await uow.Bookings.GetByFlightIdAsync(request.FlightId, cancellationToken);
        
        var bookingModels = flightBookings.Select(BookingModel.FromBooking).ToList();
        var response = new GetBookingsByFlightIdResponse(bookingModels);
        
        return ApiResponse<GetBookingsByFlightIdResponse>.Ok(response);
    }
}