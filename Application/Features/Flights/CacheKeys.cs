namespace Application.Features.Flights;

public static class CacheKeys
{
    public static readonly string AvailableKeyFormat = "flights:available:origin:{0}:destination:{1}:departureDate:{2}";
    public static readonly string BookingLockKeyFormat = "booking:lock:flight_id:{0}";
    public static readonly string FlightSeatsKeyFormat = "flights:seats:flight_id:{0}";
}