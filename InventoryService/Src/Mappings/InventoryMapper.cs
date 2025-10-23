using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryService.Src.Dtos;
using InventoryService.Src.Models;
using InventoryService.Src.Shared.Messages;

namespace InventoryService.Src.Mappings
{
    public static class InventoryMapper
    {
        public static ItemDto ToItemDto(this Inventory inventory)
        {
            return new ItemDto
            {
                ProductId = inventory.ProductId,
                ProductName = inventory.ProductName,
                ProductCategory = inventory.ProductCategory,
                StockQuantity = inventory.StockQuantity,
                ProductStatus = inventory.ProductStatus,
                ThresholdLimit = inventory.ThresholdLimit
            };
        }

        public static UpdateOperationDto ToUpdateOperationDto(this Inventory inventory, UpdateStockDto updateStockDto, int previousStock)
        {
            return new UpdateOperationDto
            {
                ProductId = inventory.ProductId,
                ProductName = inventory.ProductName,
                ProductCategory = inventory.ProductCategory,
                PreviousStock = previousStock,
                UpdatedStock = inventory.StockQuantity,
                Operation = updateStockDto.Operation,
                QuantityChanged = updateStockDto.Quantity,
            };
        }
    }
}