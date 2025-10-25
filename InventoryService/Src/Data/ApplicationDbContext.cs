using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryService.Src.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Src.Data
{
    /// <summary>
    /// Contexto de base de datos para la aplicación.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Constructor del contexto de base de datos.
        /// </summary>
        /// <param name="options">Opciones del contexto de base de datos.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Conjunto de inventarios.
        /// </summary>
        public DbSet<Inventory> Inventory { get; set; }

        /// <summary>
        /// Configuración del modelo de datos.
        /// </summary>
        /// <param name="modelBuilder"></param>
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
                entity.Property(e => e.ThresholdLimit).IsRequired().HasDefaultValue(0);
                entity.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("NOW()");
                entity.Property(e => e.UpdatedAt).IsRequired(false);
                entity.Property(e => e.DeletedAt).IsRequired(false);
            });
        }

        /// <summary>
        /// Guardar cambios con actualización de timestamps.
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        /// <summary>
        /// Guardar cambios asíncronos con actualización de timestamps.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Actualizar los timestamps de creación y actualización.
        /// </summary>
        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries<Inventory>();
            var utcNow = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = utcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = utcNow;
                }
            }
        }
    }
}