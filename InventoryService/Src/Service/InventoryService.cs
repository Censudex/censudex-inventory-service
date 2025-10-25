using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryService.Src.Dtos;
using InventoryService.Src.Interface;
using InventoryService.Src.Mappings;
using InventoryService.Src.Shared.Messages;
using MassTransit;
using MassTransit.Testing;

namespace InventoryService.Src.Service
{
    /// <summary>
    /// Servicio para la gestión del inventario.
    /// </summary>
    public class InventoryService : IInventoryService
    {
        /// <summary>
        /// Repositorio de inventario.
        /// </summary>
        private readonly IInventoryRepository _inventoryRepository;
        /// <summary>
        /// Endpoint de publicación para mensajes.
        /// </summary>
        private readonly IPublishEndpoint _publishEndpoint;

        /// <summary>
        /// Constructor del servicio de inventario.
        /// </summary>
        /// <param name="inventoryRepository">Repositorio de inventario.</param>
        /// <param name="publishEndpoint">Endpoint de publicación para mensajes.</param>
        public InventoryService(IInventoryRepository inventoryRepository, IPublishEndpoint publishEndpoint)
        {
            _inventoryRepository = inventoryRepository;
            _publishEndpoint = publishEndpoint;
        }

        /// <summary>
        /// Actualiza el stock de un artículo de inventario.
        /// </summary>
        /// <param name="id">ID del artículo de inventario.</param>
        /// <param name="updateStockDto">Datos de actualización del stock.</param>
        /// <returns>Resultado de la operación de actualización.</returns>
        public async Task<UpdateOperationDto> UpdateInventoryItemStockAsync(Guid id, UpdateStockDto updateStockDto)
        {
            var inventoryItem = await _inventoryRepository.GetInventoryEntityByIdAsync(id);

            if (inventoryItem == null)
            {
                return new UpdateOperationDto
                {
                    Success = false,
                    Message = "Inventory item not found."
                };
            }

            int previousStock = inventoryItem.StockQuantity;

            if (updateStockDto.Operation == "increase")
            {
                inventoryItem.StockQuantity += updateStockDto.Quantity;
            }
            else if (updateStockDto.Operation == "decrease")
            {
                if (inventoryItem.StockQuantity < updateStockDto.Quantity)
                {
                    return new UpdateOperationDto
                    {
                        Success = false,
                        Message = "Insufficient stock to decrease."
                    };
                }
                inventoryItem.StockQuantity -= updateStockDto.Quantity;
            }
            else
            {
                return new UpdateOperationDto
                {
                    Success = false,
                    Message = "Invalid operation type."
                };
            }

            if (inventoryItem.StockQuantity < 0)
            {
                return new UpdateOperationDto
                {
                    Success = false,
                    Message = "Stock quantity cannot be negative."
                };
            }

            var updateOperationDto = InventoryMapper.ToUpdateOperationDto(inventoryItem, updateStockDto, previousStock);

            string? alert = null;

            if (inventoryItem.StockQuantity < inventoryItem.ThresholdLimit)
            {
                alert = "Stock below minimum threshold.";
                updateOperationDto.Alert = alert;

                var stockAlertMessage = new StockAlertMessage
                {
                    ProductId = inventoryItem.ProductId,
                    ProductName = inventoryItem.ProductName,
                    ProductCategory = inventoryItem.ProductCategory,
                    CurrentStock = inventoryItem.StockQuantity,
                    ThresholdLimit = inventoryItem.ThresholdLimit,  
                    Text = $"Alert: Stock for product '{inventoryItem.ProductName}' (ID: {inventoryItem.ProductId}) is below the threshold limit. Current stock: {inventoryItem.StockQuantity}."
                };

                await _publishEndpoint.Publish(stockAlertMessage);
            }

            updateOperationDto.Success = true;
            updateOperationDto.Message = "Stock updated successfully.";

            await _inventoryRepository.UpdateInventoryItem(inventoryItem);
            return updateOperationDto;
        }

    }
}