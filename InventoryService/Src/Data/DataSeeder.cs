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
                    new Inventory { ProductName = "Laptop ASUS TUF", ProductCategory = "Electronics", StockQuantity = 100, ProductStatus = true, ThresholdLimit = 10 },
                    new Inventory { ProductName = "Wireless Mouse", ProductCategory = "Accessories", StockQuantity = 200, ProductStatus = true, ThresholdLimit = 20 },
                    new Inventory { ProductName = "Mechanical Keyboard", ProductCategory = "Accessories", StockQuantity = 300, ProductStatus = true, ThresholdLimit = 30 },
                    new Inventory { ProductName = "Gaming Headset", ProductCategory = "Audio", StockQuantity = 150, ProductStatus = true, ThresholdLimit = 15 },
                    new Inventory { ProductName = "27-inch Monitor", ProductCategory = "Electronics", StockQuantity = 80, ProductStatus = true, ThresholdLimit = 8 },
                    new Inventory { ProductName = "External SSD 1TB", ProductCategory = "Storage", StockQuantity = 120, ProductStatus = true, ThresholdLimit = 12 },
                    new Inventory { ProductName = "USB-C Hub", ProductCategory = "Peripherals", StockQuantity = 250, ProductStatus = true, ThresholdLimit = 25 },
                    new Inventory { ProductName = "Webcam HD", ProductCategory = "Video", StockQuantity = 170, ProductStatus = true, ThresholdLimit = 17 },
                    new Inventory { ProductName = "Smartphone Stand", ProductCategory = "Accessories", StockQuantity = 400, ProductStatus = true, ThresholdLimit = 40 },
                    new Inventory { ProductName = "Bluetooth Speaker", ProductCategory = "Audio", StockQuantity = 220, ProductStatus = true, ThresholdLimit = 22 }
                };

                await _dbContext.Inventory.AddRangeAsync(inventoryItems);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}