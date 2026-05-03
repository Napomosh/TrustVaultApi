using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrustDrop.Document.Models;

namespace TrustDrop.Document.Configurations;

public class DocumentModelConfiguration : IEntityTypeConfiguration<DocumentModel>
{
    public void Configure(EntityTypeBuilder<DocumentModel> builder)
    {
        builder.ToTable("document");

        builder.Property(d => d.TenantId)
            .HasColumnName("document_tenant_id")
            .IsRequired();

        builder.HasOne(d => d.Tenant)
            .WithMany()
            .HasForeignKey(d => d.TenantId);

        builder.Property(d => d.Name)
            .HasColumnName("document_name")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(d => d.OwnerId)
            .HasColumnName("document_owner_id")
            .IsRequired();

        builder.HasOne(d => d.Owner)
            .WithMany()
            .HasForeignKey(d => d.OwnerId);

        builder.Property(d => d.PolicyId)
            .HasColumnName("document_policy_id")
            .IsRequired();

        builder.HasOne(d => d.Policy)
            .WithMany()
            .HasForeignKey(d => d.PolicyId);

        builder.Property(d => d.Size)
            .HasColumnName("document_size")
            .IsRequired();

        builder.Property(d => d.Hash)
            .HasColumnName("document_hash")
            .HasMaxLength(1024)
            .IsRequired();

        builder.Property(d => d.ContentType)
            .HasColumnName("document_content_type")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(d => d.Status)
            .HasColumnName("document_status")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(d => d.StorageKey)
            .HasColumnName("document_storage_key")
            .HasMaxLength(1024)
            .IsRequired();

        builder.Property(d => d.ScannedAt)
            .HasColumnName("document_scanned_at");

        builder.Property(d => d.ExpiresAt)
            .HasColumnName("document_expires_at")
            .IsRequired();
    }
}
