namespace Application.Features.Passengers.Models;

public class PassengerModel
{
    public required string Fullname { get; set; }
    public required string Email { get; set; }
    public required string PassportNumber { get; set; }
    public string? PhoneNumber { get; set; }
}