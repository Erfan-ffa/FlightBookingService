using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class BookingEntityConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);

        builder.HasOne(b => b.Flight)
            .WithMany()
            .HasForeignKey(b => b.FlightId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Passenger)
            .WithMany()
            .HasForeignKey(b => b.PassengerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(b => b.FlightId);
        builder.HasIndex(b => b.PassengerId);
    }
}