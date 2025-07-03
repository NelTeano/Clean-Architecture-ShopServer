using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyServer.Core.Entities;
namespace MyServer.Infrastructure.Data
{
    public class ApplicationContextDB(DbContextOptions<ApplicationContextDB> options) : DbContext(options)
    {
        public DbSet<UserEntity> User { get; set; }
        public DbSet<OrderEntity> Order { get; set; }
        public DbSet<OrderItemEntity> OrderItem { get; set; }
        public DbSet<PaymentEntity> Payment { get; set; }
        public DbSet<ProductEntity> Product { get; set; }
        public DbSet<CategoryEntity> Category { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // OrderEntity decimal configurations
            modelBuilder.Entity<OrderEntity>()
                .Property(e => e.SubTotal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderEntity>()
                .Property(e => e.Tax)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderEntity>()
                .Property(e => e.Total)
                .HasPrecision(18, 2);

            // OrderItemEntity decimal configurations
            modelBuilder.Entity<OrderItemEntity>()
                .Property(e => e.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItemEntity>()
                .Property(e => e.TotalPrice)
                .HasPrecision(18, 2);

            // PaymentEntity decimal configurations
            modelBuilder.Entity<PaymentEntity>()
                .Property(e => e.Amount)
                .HasPrecision(18, 2);

            // ProductEntity decimal configurations
            modelBuilder.Entity<ProductEntity>()
                .Property(e => e.Price)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}
