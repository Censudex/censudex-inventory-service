using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryService.Src.Data;
using InventoryService.Src.Dtos;
using InventoryService.Src.Interface;
using InventoryService.Src.Mappings;
using InventoryService.Src.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Src.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly ApplicationDbContext _context;

        public InventoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Inventory>> GetAllInventoryItemsAsync()
        {
            return await _context.Inventory.ToListAsync();
        }

        public async Task<ItemDto> GetInventoryItemByIdAsync(Guid id)
        {
            var inventoryItem = await _context.Inventory.FindAsync(id) ?? throw new KeyNotFoundException("Inventory item not found.");
            return InventoryMapper.ToItemDto(inventoryItem);
        }

        public async Task<UpdateOperationDto> UpdateInventoryItemStockAsync(Guid id, UpdateStockDto updateStockDto)
        {
            var inventoryItem = await _context.Inventory.FindAsync(id) ?? throw new KeyNotFoundException("Inventory item not found.");

            int previousStock = inventoryItem.StockQuantity;

            if (updateStockDto.Operation.Equals("increase", StringComparison.OrdinalIgnoreCase))
            {
                inventoryItem.StockQuantity += updateStockDto.Quantity;
            }
            else if (updateStockDto.Operation.Equals("decrease", StringComparison.OrdinalIgnoreCase))
            {
                if (inventoryItem.StockQuantity < updateStockDto.Quantity)
                {
                    throw new InvalidOperationException("Insufficient stock to decrease.");
                }
                inventoryItem.StockQuantity -= updateStockDto.Quantity;
            }
            else
            {
                throw new ArgumentException("Invalid operation. Must be 'increase' or 'decrease'.");
            }

            await _context.SaveChangesAsync();

            return InventoryMapper.ToUpdateOperationDto(inventoryItem, updateStockDto, previousStock);
        }
    }
}