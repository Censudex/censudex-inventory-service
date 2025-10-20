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
    }
}