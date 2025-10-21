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
        public Task<ItemDto> GetInventoryItemByIdAsync(Guid id);
        public Task<UpdateOperationDto> UpdateInventoryItemStockAsync(Guid id, UpdateStockDto updateStockDto);
    }
}