using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrustDrop.Common.Database;
using TrustDrop.User.Models;

namespace TrustDrop.User.Configurations;

public class UserModelConfiguration : IEntityTypeConfiguration<UserModel>
{
    public void Configure(EntityTypeBuilder<UserModel> builder)
    {
        builder.ToTable("user");

        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName(DbIndexes.INDEX_USER_EMAIL_UNIQUE);

        builder.HasIndex(u => u.Username)
            .IsUnique()
            .HasDatabaseName(DbIndexes.INDEX_USER_USERNAME_UNIQUE);

        builder.Property(u => u.Username)
            .HasColumnName("user_username")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasColumnName("user_email")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.PasswordHash)
            .HasColumnName("user_password_hash")
            .HasColumnType("bytea")
            .HasMaxLength(32);

        builder.Property(u => u.Salt)
            .HasColumnName("user_salt")
            .HasColumnType("bytea")
            .HasMaxLength(16);

        builder.Property(u => u.Role)
            .HasColumnName("user_role");

        builder.Property(u => u.LastLogin)
            .HasColumnName("user_last_login");
    }
}
