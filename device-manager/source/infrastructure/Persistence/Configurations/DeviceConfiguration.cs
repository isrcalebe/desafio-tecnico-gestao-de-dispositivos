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

        builder.OwnsOne(d => d.SerialNumber, serialNumberBuilder =>
        {
            serialNumberBuilder.Property(s => s.Value)
                .HasColumnName("SerialNumber")
                .IsRequired()
                .HasMaxLength(50);

            serialNumberBuilder.HasIndex(s => s.Value)
                .IsUnique();
        });

        builder.OwnsOne(d => d.IMEI, imeiBuilder =>
        {
            imeiBuilder.Property(i => i.Value)
                .HasColumnName("IMEI")
                .IsRequired()
                .HasMaxLength(15);

            imeiBuilder.HasIndex(i => i.Value)
                .IsUnique();
        });

        builder.HasMany(d => d.Events)
            .WithOne(e => e.Device)
            .HasForeignKey(e => e.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.Client)
            .WithMany(c => c.Devices)
            .HasForeignKey(d => d.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(d => d.ActivatedAt)
            .IsRequired(false);

        builder.Property(d => d.CreatedAt)
            .IsRequired();

        builder.Property(d => d.LastUpdatedAt)
            .IsRequired();
    }
}
