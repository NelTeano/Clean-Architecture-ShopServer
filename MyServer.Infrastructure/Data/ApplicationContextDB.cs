using Microsoft.EntityFrameworkCore;
using MyServer.Core.Entities;
using MyServer.Core.Entities.ProductEntities;
namespace MyServer.Infrastructure.Data
{
    public class ApplicationContextDB(DbContextOptions<ApplicationContextDB> options) : DbContext(options)
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<PaymentEntity> Payments { get; set; }
        public DbSet<VariantEntity> Variants { get; set; }
        public DbSet<SubVariantEntity> SubVariants { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<ProductItemEntity> ProductItems { get; set; }
        public DbSet<ProductSizeEntity> ProductSizes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // PaymentEntity decimal configurations
            modelBuilder.Entity<PaymentEntity>()
                .Property(e => e.Amount)
                .HasPrecision(18, 2);

            // Configure relationships of products infos
            modelBuilder.Entity<SubVariantEntity>()
                .HasOne(sv => sv.Variant)
                .WithMany(v => v.SubVariants)
                .HasForeignKey(sv => sv.VariantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CategoryEntity>()
                .HasOne(c => c.SubVariant)
                .WithMany(sv => sv.Categories)
                .HasForeignKey(c => c.SubVariantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductItemEntity>()
                .HasOne(pi => pi.Category)
                .WithMany(c => c.Items)
                .HasForeignKey(pi => pi.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductSizeEntity>()
                .HasOne(ps => ps.ProductItem)
                .WithMany(pi => pi.Sizes)
                .HasForeignKey(ps => ps.ProductItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure indexes for better performance
            modelBuilder.Entity<SubVariantEntity>()
                .HasIndex(sv => sv.VariantId);

            modelBuilder.Entity<CategoryEntity>()
                .HasIndex(c => c.SubVariantId);

            modelBuilder.Entity<ProductItemEntity>()
                .HasIndex(pi => pi.CategoryId);

            modelBuilder.Entity<ProductSizeEntity>()
                .HasIndex(ps => ps.ProductItemId);

            // Configure timestamp properties - use this approach if removing property defaults
            ConfigureTimestampProperties(modelBuilder);
        }

        private void ConfigureTimestampProperties(ModelBuilder modelBuilder)
        {
            // Configure CreatedAt to be set only on add
            var entityTypes = modelBuilder.Model.GetEntityTypes()
                .Where(t => t.ClrType.GetProperty("CreatedAt") != null);

            foreach (var entityType in entityTypes)
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property("CreatedAt")
                    .HasDefaultValueSql("GETUTCDATE()") // SQL Server
                    .ValueGeneratedOnAdd();

                // For other databases, use:
                // .HasDefaultValueSql("NOW()") // PostgreSQL
                // .HasDefaultValueSql("datetime('now')") // SQLite
            }
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                var entityType = entry.Entity.GetType();
                var now = DateTime.UtcNow;

                // Only set CreatedAt for new entities
                if (entry.State == EntityState.Added)
                {
                    var createdAtProperty = entityType.GetProperty("CreatedAt");
                    if (createdAtProperty != null)
                    {
                        entry.Property("CreatedAt").CurrentValue = now;
                    }
                }

                // Always set UpdatedAt for both new and modified entities
                var updatedAtProperty = entityType.GetProperty("UpdatedAt");
                if (updatedAtProperty != null)
                {
                    entry.Property("UpdatedAt").CurrentValue = now;
                }
            }
        }
    }
}