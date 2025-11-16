using System.Text.RegularExpressions;
using FluentValidation;

namespace Application.Features.Bookings.Book;

public class CreateBookingValidator : AbstractValidator<CreateBookingRequest>
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);
    
    public CreateBookingValidator()
    {
        RuleFor(x => x.Passenger.Fullname)
            .NotEmpty()
            .WithMessage("Name cannot be empty");
        
        RuleFor(x => x.Passenger.PassportNumber)
            .Length(8)
            .WithMessage("Invalid Passport Number.");
        
        RuleFor(x => x.Passenger.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .Must(BeAValidEmail)
            .WithMessage("Invalid email format")
            .MaximumLength(255)
            .WithMessage("Email must not exceed 255 characters");
    }
    
    private bool BeAValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        return EmailRegex.IsMatch(email);
    }
}