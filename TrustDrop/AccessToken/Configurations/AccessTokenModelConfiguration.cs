using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrustDrop.AccessToken.Models;

namespace TrustDrop.AccessToken.Configurations;

public class AccessTokenModelConfiguration : IEntityTypeConfiguration<AccessTokenModel>
{
    public void Configure(EntityTypeBuilder<AccessTokenModel> builder)
    {
        builder.ToTable("access_token");

        builder.Property(a => a.TenantId)
            .HasColumnName("access_token_tenant_id")
            .IsRequired();

        builder.HasOne(a => a.Tenant)
            .WithMany()
            .HasForeignKey(a => a.TenantId);

        builder.Property(a => a.DocumentId)
            .HasColumnName("access_token_document_id")
            .IsRequired();

        builder.HasOne(a => a.Document)
            .WithMany()
            .HasForeignKey(a => a.DocumentId);

        builder.Property(a => a.TokenHash)
            .HasColumnName("access_token_hash")
            .HasMaxLength(1024)
            .IsRequired();

        builder.Property(a => a.OneTime)
            .HasColumnName("access_token_one_time")
            .IsRequired();

        builder.Property(a => a.TtlSeconds)
            .HasColumnName("access_token_ttl_seconds")
            .IsRequired();

        builder.Property(a => a.UsedCount)
            .HasColumnName("access_token_used_count");

        builder.Property(a => a.LastUsedAt)
            .HasColumnName("access_token_last_used_at");
    }
}
