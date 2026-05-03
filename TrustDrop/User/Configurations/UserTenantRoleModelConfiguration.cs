using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrustDrop.User.Models;

namespace TrustDrop.User.Configurations;

public class UserTenantRoleModelConfiguration : IEntityTypeConfiguration<UserTenantRoleModel>
{
    public void Configure(EntityTypeBuilder<UserTenantRoleModel> builder)
    {
        builder.ToTable("user_tenant_role");

        builder.HasIndex(u => new { u.UserId, u.TenantId })
            .IsUnique();

        builder.Property(u => u.UserId)
            .HasColumnName("user_tenant_role_user_id")
            .IsRequired();

        builder.HasOne(u => u.User)
            .WithMany()
            .HasForeignKey(u => u.UserId);

        builder.Property(u => u.TenantId)
            .HasColumnName("user_tenant_role_tenant_id")
            .IsRequired();

        builder.HasOne(u => u.Tenant)
            .WithMany()
            .HasForeignKey(u => u.TenantId);

        builder.Property(u => u.Role)
            .HasColumnName("user_tenant_role_role")
            .IsRequired();
    }
}
