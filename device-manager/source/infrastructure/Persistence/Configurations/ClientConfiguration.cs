using DeviceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeviceManager.Infrastructure.Persistence.Configurations;

public sealed class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(255);

        builder.HasIndex(c => c.Email)
            .IsUnique();

        builder.Property(c => c.Phone)
            .HasMaxLength(20);

        builder.Property(c => c.Status)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.LastUpdatedAt)
            .IsRequired();

        builder.HasMany(c => c.Devices)
            .WithOne(d => d.Client)
            .HasForeignKey(d => d.ClientId);
    }
}
