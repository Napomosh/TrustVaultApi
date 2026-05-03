using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrustDrop.Auth.Models;
using TrustDrop.Common.Database;

namespace TrustDrop.Auth.Configurations;

public class RefreshTokenModelConfiguration : IEntityTypeConfiguration<RefreshTokenModel>
{
    public void Configure(EntityTypeBuilder<RefreshTokenModel> builder)
    {
        builder.ToTable("refresh_token");

        builder.HasIndex(r => r.TokenHash)
            .IsUnique()
            .HasDatabaseName(DbIndexes.INDEX_REFRESH_TOKEN_TOKEN_HASH_UNIQUE);

        builder.HasIndex(r => new { r.UserId, r.TenantId })
            .HasDatabaseName(DbIndexes.INDEX_REFRESH_TOKEN_USER_TENANT);

        builder.Property(r => r.UserId)
            .HasColumnName("refresh_token_user_id")
            .IsRequired();

        builder.HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId);

        builder.Property(r => r.TenantId)
            .HasColumnName("refresh_token_tenant_id");

        builder.HasOne(r => r.Tenant)
            .WithMany()
            .HasForeignKey(r => r.TenantId);

        builder.Property(r => r.TokenHash)
            .HasColumnName("refresh_token_hash")
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(r => r.ExpiresAt)
            .HasColumnName("refresh_token_expires_at")
            .IsRequired();

        builder.Property(r => r.RevokedAt)
            .HasColumnName("refresh_token_revoked_at");

        builder.Ignore(r => r.IsExpired);
        builder.Ignore(r => r.IsRevoked);
    }
}
