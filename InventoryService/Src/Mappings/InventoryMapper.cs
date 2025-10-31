using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryService.Src.Dtos;
using InventoryService.Src.Models;
using InventoryService.Src.Shared.Messages;

namespace InventoryService.Src.Mappings
{
    /// <summary>
    /// Clase estática para mapear entre modelos de inventario y DTOs.
    /// </summary>
    public static class InventoryMapper
    {
        /// <summary>
        /// Mapea un objeto Inventory a un ItemDto.
        /// </summary>
        /// <param name="inventory">Entidad de inventario.</param>
        /// <returns>DTO de elemento de inventario.</returns>
        public static ItemDto ToItemDto(this Inventory inventory)
        {
            return new ItemDto
            {
                ProductId = inventory.ProductId,
                ProductName = inventory.ProductName,
                ProductCategory = inventory.ProductCategory,
                StockQuantity = inventory.StockQuantity,
                ProductStatus = inventory.ProductStatus,
                ThresholdLimit = inventory.ThresholdLimit,
                CreatedAt = inventory.CreatedAt,
                UpdatedAt = inventory.UpdatedAt,
            };
        }

        /// <summary>
        /// Mapea un objeto Inventory y UpdateStockDto a un UpdateOperationDto.
        /// </summary>
        /// <param name="inventory">Entidad de inventario.</param>
        /// <param name="updateStockDto">DTO de actualización de stock.</param>
        /// <param name="previousStock">Cantidad de stock anterior.</param>
        /// <returns>DTO de operación de actualización.</returns>
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