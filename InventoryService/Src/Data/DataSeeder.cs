using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryService.Src.Models;

namespace InventoryService.Src.Data
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _dbContext;

        public DataSeeder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SeedAsync()
        {
            if (!_dbContext.Inventory.Any())
            {
                var inventoryItems = new List<Inventory>
                {
                    new Inventory { ProductName = "Product A", ProductCategory = "Category 1", StockQuantity = 100, ProductStatus = true },
                    new Inventory { ProductName = "Product B", ProductCategory = "Category 2", StockQuantity = 200, ProductStatus = true },
                    new Inventory { ProductName = "Product C", ProductCategory = "Category 3", StockQuantity = 300, ProductStatus = true }
                };

                await _dbContext.Inventory.AddRangeAsync(inventoryItems);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}