using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryService.Src.Dtos;
using InventoryService.Src.Models;

namespace InventoryService.Src.Interface
{
    public interface IInventoryRepository
    {
        public Task<IEnumerable<Inventory>> GetAllInventoryItemsAsync();
        public Task<ItemDto> GetInventoryItemDtoByIdAsync(Guid id);
        public Task<Inventory> GetInventoryEntityByIdAsync(Guid id);
        public Task<Inventory> UpdateInventoryItem(Inventory item);
    }
}