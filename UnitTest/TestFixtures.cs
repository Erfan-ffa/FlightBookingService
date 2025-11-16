using Application.Contracts;
using Application.Contracts.Repositories;
using Application.Features.Bookings.Book;
using Application.Features.Passengers.Models;
using Microsoft.Extensions.Logging;
using Moq;
using RedLockNet;

namespace UnitTest;

public class TestFixtures
{
    public Mock<IUnitOfWork> MockUow { get; }
    public Mock<ICacheService> MockCacheService { get; }
    public Mock<IDistributedLockFactory> MockRedLock { get; }
    public Mock<ILogger<CreateBookingHandler>> MockLogger { get; }
    public Mock<IFlightRepository> MockFlightRepository { get; }
    public Mock<IBookingRepository> MockBookingRepository { get; }

    public TestFixtures()
    {
        MockUow = new Mock<IUnitOfWork>();
        MockCacheService = new Mock<ICacheService>();
        MockRedLock = new Mock<IDistributedLockFactory>();
        MockLogger = new Mock<ILogger<CreateBookingHandler>>();
        MockFlightRepository = new Mock<IFlightRepository>();
        MockBookingRepository = new Mock<IBookingRepository>();

        MockUow.Setup(x => x.Flights).Returns(MockFlightRepository.Object);
        MockUow.Setup(x => x.Bookings).Returns(MockBookingRepository.Object);
    }

    public CreateBookingHandler CreateHandler()
    {
        return new CreateBookingHandler(
            MockUow.Object,
            MockCacheService.Object,
            MockRedLock.Object,
            MockLogger.Object
        );
    }

    public PassengerModel CreatePassenger(string fullname, string email, string passportNumber, string? phoneNumber)
    {
        return new PassengerModel
        {
            Fullname = fullname,
            Email = email,
            PassportNumber = passportNumber,
            PhoneNumber = phoneNumber
        };
    }
}