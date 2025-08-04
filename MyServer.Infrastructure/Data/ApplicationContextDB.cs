using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyServer.Core.Entities;
using MyServer.Core.Entities.ProductEntities;


namespace MyServer.Infrastructure.Data
{
    public class ApplicationContextDB(DbContextOptions<ApplicationContextDB> options) : DbContext(options)
    {
        public DbSet<UserEntity> User { get; set; }
        public DbSet<PaymentEntity> Payment { get; set; }

        public DbSet<VariantEntity> Variants { get; set; }
        public DbSet<SubVariantEntity> SubVariants { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<ProductSizeEntity> ProductItems { get; set; }

        public DbSet<ProductItemEntity> ProductSizes { get; set; }


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


            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType.GetProperties()
                    .Where(p => p.Name == "UpdatedAt");

                foreach (var property in properties)
                {
                    modelBuilder.Entity(entityType.Name)
                        .Property(property.Name)
                        .ValueGeneratedOnAddOrUpdate();
                }
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
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.Entity.GetType().GetProperty("UpdatedAt") != null)
                {
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                }
            }
        }


    }
}
