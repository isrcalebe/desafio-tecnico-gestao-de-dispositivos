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

        builder.OwnsOne(d => d.Serial, serialBuilder =>
        {
            serialBuilder.Property(s => s)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("SerialNumber");

            serialBuilder.HasIndex(s => s)
                .IsUnique();
        });

        builder.OwnsOne(d => d.IMEI, imeiBuilder =>
        {
            imeiBuilder.Property(i => i)
                .IsRequired()
                .HasMaxLength(15)
                .HasColumnName("IMEI");

            imeiBuilder.HasIndex(i => i)
                .IsUnique();
        });

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
