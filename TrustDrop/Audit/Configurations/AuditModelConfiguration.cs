using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrustDrop.Audit.Models;

namespace TrustDrop.Audit.Configurations;

public class AuditModelConfiguration : IEntityTypeConfiguration<AuditModel>
{
    public void Configure(EntityTypeBuilder<AuditModel> builder)
    {
        builder.ToTable("audit");

        builder.Property(a => a.TenantId)
            .HasColumnName("audit_tenant_id")
            .IsRequired();

        builder.HasOne(a => a.Tenant)
            .WithMany()
            .HasForeignKey(a => a.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(a => a.ActorId)
            .HasColumnName("audit_actor_id")
            .IsRequired();

        builder.HasOne(a => a.Actor)
            .WithMany()
            .HasForeignKey(a => a.ActorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(a => a.DocumentId)
            .HasColumnName("audit_document_id")
            .IsRequired();

        builder.HasOne(a => a.Document)
            .WithMany()
            .HasForeignKey(a => a.DocumentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(a => a.Action)
            .HasColumnName("audit_action")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(a => a.IpAddress)
            .HasColumnName("audit_ip")
            .HasMaxLength(45)
            .IsRequired();

        builder.Property(a => a.UserAgent)
            .HasColumnName("audit_ua")
            .HasMaxLength(1024)
            .IsRequired();

        builder.Property(a => a.Details)
            .HasColumnName("audit_details")
            .HasMaxLength(2048);
    }
}
