using DeviceManager.Domain.Entities;
using DeviceManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeviceManager.Infrastructure.Persistence.Configurations;

public sealed class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("Events");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Type)
            .HasConversion(
                e => e.ToString(),
                e => Enum.Parse<Event.EventType>(e)
            );

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.LastUpdatedAt)
            .IsRequired();
    }
}
