using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryService.Src.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Src.Data
{
    public class ApplicationDbContext : DbContext   
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Inventory> Inventories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasKey(e => e.ProductId);
                entity.Property(e => e.ProductName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ProductCategory).IsRequired().HasMaxLength(100);
                entity.Property(e => e.StockQuantity).IsRequired();
                entity.Property(e => e.ProductStatus).IsRequired().HasDefaultValue(true);
            });

            modelBuilder.Entity<Inventory>().HasData(
                new Inventory
                {
                    ProductId = new Guid("a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d"),
                    ProductName = "Sample Product 1",
                    ProductCategory = "Category A",
                    StockQuantity = 50,
                    ProductStatus = true
                },
                new Inventory
                {
                    ProductId = new Guid("b1c2d3e4-f5a6-4b5c-9d0e-1f2a3b4c5d6e"),
                    ProductName = "Sample Product 2",
                    ProductCategory = "Category B",
                    StockQuantity = 30,
                    ProductStatus = true
                }
            );
        }
    }
}