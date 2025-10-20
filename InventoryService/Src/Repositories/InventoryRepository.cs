using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryService.Src.Data;
using InventoryService.Src.Interface;
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
    }
}