using Domain.Common;

namespace Domain.Entities;

public class Passenger : AuditableEntity
{
    public string Fullname { get; set; }
    public string Email { get; set; }
    public string PassportNumber { get; set; }
    public string? PhoneNumber { get; set; }
}