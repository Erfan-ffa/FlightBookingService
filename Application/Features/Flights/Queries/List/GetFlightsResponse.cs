using Application.Features.Flights.Models;

namespace Application.Features.Flights.Queries.List;

public class GetFlightsResponse
{
    public List<FlightDto> FlightItems { get; set; }

    public GetFlightsResponse(List<FlightDto> flightItems)
    {
        FlightItems =  flightItems;
    }
}