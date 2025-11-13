using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class FlightEntityConfiguration : IEntityTypeConfiguration<Flight>
{
    public void Configure(EntityTypeBuilder<Flight> builder)
    {
        builder.Property(f => f.FlightNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.Origin)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.Destination)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.Price).HasPrecision(18, 2);

        builder.HasIndex(f => f.FlightNumber);
    }
}