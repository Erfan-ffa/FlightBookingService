namespace Application.ApiMessages;

public static class FlightMessages
{
    public static string AlreadyExists = "FlightAlreadyExists";
    public static string NotFound = "FlightNotFound";
    public static string SeatsNotAvailable = "SeatsNotAvailable";
    public static string SeatAlreadyBooked = "SeatAlreadyBooked";
    public static string InternalError = "InternalError";
    public static string CanNotDecreaseAvailableSeats = "CanNotDecreaseAvailableSeats";
}