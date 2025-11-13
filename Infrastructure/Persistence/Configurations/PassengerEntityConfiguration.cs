using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PassengerEntityConfiguration : IEntityTypeConfiguration<Passenger>
{
    public void Configure(EntityTypeBuilder<Passenger> builder)
    {
        builder.Property(x => x.Email).IsRequired().HasMaxLength(150);
        builder.Property(x => x.PassportNumber).IsRequired().HasMaxLength(9);
        builder.Property(x => x.Fullname).IsRequired().HasMaxLength(250);
    }
}