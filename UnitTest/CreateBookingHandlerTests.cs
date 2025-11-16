using Application.ApiMessages;
using Application.Features.Bookings.Book;
using Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace UnitTest;

public class CreateBookingHandlerTests
{
    private readonly TestFixtures _fixtures;
    private readonly CreateBookingHandler _handler;

    public CreateBookingHandlerTests()
    {
        _fixtures = new TestFixtures();
        _handler = _fixtures.CreateHandler();
    }

    [Fact]
    public async Task Handle_WhenFlightNotFound_ReturnsBadRequest()
    {
        var passenger = _fixtures.CreatePassenger("Ali", "Ali@gmail.com", "A1234567", null);
        var request = new CreateBookingRequest(123, passenger);
        var cancellationToken = CancellationToken.None;

        _fixtures.MockFlightRepository
            .Setup(x => x.GetByIdAsync(request.FlightId, cancellationToken))
            .ReturnsAsync((Flight?)null);

        var result = await _handler.Handle(request, cancellationToken);

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        result.Message.Should().Be(FlightMessages.NotFound);
        result.Data.Should().BeNull();

        _fixtures.MockCacheService.Verify(
            x => x.PopAsync<int?>(It.IsAny<string>()), 
            Times.Never
        );

        _fixtures.MockBookingRepository.Verify(
            x => x.Add(It.IsAny<Booking>()), 
            Times.Never
        );

        _fixtures.MockUow.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), 
            Times.Never
        );
    }
}