using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrustDrop.Tenant.Models;

namespace TrustDrop.Tenant.Configurations;

public class TenantModelConfiguration : IEntityTypeConfiguration<TenantModel>
{
    public void Configure(EntityTypeBuilder<TenantModel> builder)
    {
        builder.ToTable("tenant");

        builder.Property(t => t.Name)
            .HasColumnName("tenant_name")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(t => t.OwnerId)
            .HasColumnName("tenant_owner_id")
            .IsRequired();

        builder.HasOne(t => t.Owner)
            .WithMany()
            .HasForeignKey(t => t.OwnerId);
    }
}
