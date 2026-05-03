using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrustDrop.Policy.Models;

namespace TrustDrop.Policy.Configurations;

public class PolicyModelConfiguration : IEntityTypeConfiguration<PolicyModel>
{
    public void Configure(EntityTypeBuilder<PolicyModel> builder)
    {
        builder.ToTable("policy");

        builder.Property(p => p.TenantId)
            .HasColumnName("policy_tenant_id")
            .IsRequired();

        builder.HasOne(p => p.Tenant)
            .WithMany()
            .HasForeignKey(p => p.TenantId);

        builder.Property(p => p.AllowedUserId)
            .HasColumnName("policy_allowed_user_id");

        builder.HasOne(p => p.AllowedUser)
            .WithMany()
            .HasForeignKey(p => p.AllowedUserId);

        builder.Property(p => p.AllowedDomain)
            .HasColumnName("policy_allowed_domain")
            .HasMaxLength(255);

        builder.Property(p => p.MaxDownloads)
            .HasColumnName("policy_max_downloads");

        builder.Property(p => p.ValidUntil)
            .HasColumnName("policy_valid_until");
    }
}
