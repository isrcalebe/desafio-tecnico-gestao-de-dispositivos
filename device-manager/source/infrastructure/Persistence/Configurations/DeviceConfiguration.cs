using DeviceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeviceManager.Infrastructure.Persistence.Configurations;

public sealed class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.ToTable("Devices");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Serial)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("SerialNumber");

        builder.HasIndex(d => d.Serial)
            .IsUnique();

        builder.Property(d => d.IMEI)
            .IsRequired()
            .HasMaxLength(15);

        builder.HasIndex(d => d.IMEI)
            .IsUnique();

        builder.HasMany(d => d.Events)
            .WithOne(e => e.Device)
            .HasForeignKey(e => e.DeviceId);

        /*builder.Property(d => d.ActivatedAt)
            .IsRequired();*/

        builder.Property(d => d.CreatedAt)
            .IsRequired();

        builder.Property(d => d.LastUpdatedAt)
            .IsRequired();
    }
}
