using Application.Utils;
using Domain.Entities;
using MediatR;

namespace Application.Features.Flights.Queries.List;

public class GetFlightsQuery : IRequest<ApiResponse<GetFlightsResponse>>
{
    public string Origin { get; set; }
    public string Destination { get; set; }
    public DateOnly DepartureDate { get; set; }
    public DateOnly? ArrivalDate { get; set; }
    public int? AvailableSeats { get; set; }
}