using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryService.Src.Models;

namespace InventoryService.Src.Data
{
    /// <summary>
    /// Clase seeder para poblar la base de datos con datos iniciales.
    /// </summary>
    public class DataSeeder
    {
        /// <summary>
        /// Contexto de la base de datos.
        /// </summary>
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Constructor de la clase DataSeeder.
        /// </summary>
        /// <param name="dbContext">Contexto de la base de datos.</param>
        public DataSeeder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Método para sembrar datos iniciales en la base de datos.
        /// </summary>
        /// <returns></returns>
        public async Task SeedAsync()
        {
            if (!_dbContext.Inventory.Any())
            {
                var inventoryItems = new List<Inventory>
                {
                    new Inventory { ProductName = "Product A", ProductCategory = "Category 1", StockQuantity = 100, ProductStatus = true, ThresholdLimit = 10 },
                    new Inventory { ProductName = "Product B", ProductCategory = "Category 2", StockQuantity = 200, ProductStatus = true, ThresholdLimit = 20 },
                    new Inventory { ProductName = "Product C", ProductCategory = "Category 3", StockQuantity = 300, ProductStatus = true, ThresholdLimit = 30 }
                };

                await _dbContext.Inventory.AddRangeAsync(inventoryItems);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}