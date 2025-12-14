using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TrustDrop.AccessToken.Models;
using TrustDrop.Audit.Models;
using TrustDrop.Auth.Models;
using TrustDrop.Document.Models;
using TrustDrop.Policy.Models;
using TrustDrop.Tenant.Models;
using TrustDrop.User.Models;

namespace TrustDrop.Common.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<UserModel> Users { get; set; }
    public DbSet<DocumentModel> Documents { get; set; }
    public DbSet<AccessTokenModel> AccessTokens { get; set; }
    public DbSet<AuditModel> Audits { get; set; }
    public DbSet<PolicyModel> Policies { get; set; }
    public DbSet<TenantModel> Tenants { get; set; }
    public DbSet<UserTenantRoleModel> UserTenantRoles { get; set; }
    public DbSet<RefreshTokenModel> RefreshTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) 
            return;
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseNpgsql(connectionString);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(BaseEntity).IsAssignableFrom(entityType.ClrType)) 
                continue;
            
            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var deletedAtProp = Expression.Property(parameter, nameof(BaseEntity.DeletedAt));
            var filter = Expression.Lambda(
                Expression.Equal(deletedAtProp, Expression.Constant(null)),
                parameter
            );
            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
        }

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entity.GetTableName();
            
            var idProp = entity.FindProperty(nameof(BaseEntity.Id));
            idProp?.SetColumnName($"{tableName}_id");

            var createdAtProp = entity.FindProperty(nameof(BaseEntity.CreatedAt));
            createdAtProp?.SetColumnName($"{tableName}_created_at");

            var updatedAtProp = entity.FindProperty(nameof(BaseEntity.UpdatedAt));
            updatedAtProp?.SetColumnName($"{tableName}_updated_at");
            
            var deletedAtProp = entity.FindProperty(nameof(BaseEntity.DeletedAt));
            deletedAtProp?.SetColumnName($"{tableName}_deleted_at");
        }
        
        modelBuilder.Entity<AuditModel>()
            .HasOne(a => a.Tenant)
            .WithMany()
            .HasForeignKey(a => a.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AuditModel>()
            .HasOne(a => a.Document)
            .WithMany()
            .HasForeignKey(a => a.DocumentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AuditModel>()
            .HasOne(a => a.Actor)
            .WithMany()
            .HasForeignKey(a => a.ActorId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        UpdateTimestamps();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
    
    private void UpdateTimestamps()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e is { Entity: BaseEntity, State: EntityState.Modified or EntityState.Added or EntityState.Deleted });

        var now = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            var entity = (BaseEntity)entry.Entity;

            switch (entry.State)
            {
                case EntityState.Added:
                case EntityState.Modified:
                    entity.UpdatedAt = now;
                    break;
                case EntityState.Deleted:
                    entity.DeletedAt = now;
                    entry.State = entity.AllowHardDelete ? EntityState.Deleted : EntityState.Modified;
                    break;
            }
        }
    }
}