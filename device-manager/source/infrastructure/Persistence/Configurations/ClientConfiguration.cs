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

        builder.OwnsOne(c => c.Email, emailBuilder =>
        {
            emailBuilder.Property(e => e)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("Email");

            emailBuilder.HasIndex(e => e)
                .IsUnique();
        });

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
