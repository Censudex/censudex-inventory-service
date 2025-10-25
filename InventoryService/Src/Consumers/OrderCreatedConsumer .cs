using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryService.Src.Dtos;
using InventoryService.Src.Interface;
using InventoryService.Src.Messages;
using InventoryService.Src.Shared.Messages;
using MassTransit;

namespace InventoryService.Src.Consumers
{
    /// <summary>
    /// Consumidor de mensajes de orden creada.
    /// </summary>
    public class OrderCreatedConsumer : IConsumer<OrderCreatedMessage>
    {
        /// <summary>
        /// Servicio de inventario.
        /// </summary>
        private readonly IInventoryService _inventoryService;
        /// <summary>
        /// Endpoint de publicación para enviar mensajes.
        /// </summary>
        private readonly IPublishEndpoint _publishEndpoint;

        /// <summary>
        /// Constructor del consumidor.
        /// </summary>
        /// <param name="inventoryService">Servicio de inventario.</param>
        /// <param name="publishEndpoint">Endpoint de publicación para enviar mensajes.</param>
        public OrderCreatedConsumer(IInventoryService inventoryService, IPublishEndpoint publishEndpoint)
        {
            _inventoryService = inventoryService;
            _publishEndpoint = publishEndpoint;
        }

        /// <summary>
        /// Consume el mensaje de orden creada.
        /// </summary>
        /// <param name="context">Contexto de consumo del mensaje.</param>
        /// <returns>Respuesta de la operación de consumo.</returns>
        public async Task Consume(ConsumeContext<OrderCreatedMessage> context)
        {
            var order = context.Message;
            
            Console.WriteLine($"[RABBITMQ CONSUMER] Recibida orden: {order.OrderId}");

            var failedProducts = new List<StockFailure>();
            var successfulUpdates = new List<UpdateOperationDto>();

            // Validar y procesar cada producto de la orden
            foreach (var item in order.Items)
            {
                // Usar el servicio para actualizar el stock
                var updateStockDto = new UpdateStockDto
                {
                    Operation = "decrease",
                    Quantity = item.Quantity
                };

                var result = await _inventoryService.UpdateInventoryItemStockAsync(item.ProductId, updateStockDto);

                // Si la operación falló, agregar a la lista de fallos
                if (!result.Success)
                {
                    failedProducts.Add(new StockFailure
                    {
                        ProductId = item.ProductId,
                        ProductName = result.ProductName ?? "Unknown Product",
                        RequestedQuantity = item.Quantity,
                        AvailableStock = result.PreviousStock
                    });

                    Console.WriteLine($"[RABBITMQ] Fallo al descontar stock: {result.ProductName} - {result.Message}");
                }
                else
                {
                    successfulUpdates.Add(result);
                    Console.WriteLine($"[RABBITMQ] Stock actualizado: {result.ProductName} - {result.PreviousStock} → {result.UpdatedStock}");
                }
            }

            // Si algún producto falló, publicar evento de fallo y revertir cambios
            if (failedProducts.Any())
            {
                // Revertir los productos que sí se actualizaron
                foreach (var successUpdate in successfulUpdates)
                {
                    var revertDto = new UpdateStockDto
                    {
                        Operation = "increase",
                        Quantity = successUpdate.QuantityChanged
                    };

                    await _inventoryService.UpdateInventoryItemStockAsync(
                        successUpdate.ProductId, 
                        revertDto);

                    Console.WriteLine($"[RABBITMQ] Stock revertido: {successUpdate.ProductName}");
                }

                // Publicar mensaje de orden fallida
                var failedMessage = new OrderFailedStockMessage
                {
                    OrderId = order.OrderId,
                    Reason = "Insufficient stock for one or more products",
                    FailedProducts = failedProducts,
                    FailedAt = DateTime.UtcNow
                };

                await _publishEndpoint.Publish(failedMessage);

                Console.WriteLine($"[RABBITMQ] Orden {order.OrderId} rechazada por falta de stock");
                return;
            }

            // Si todo fue exitoso
            Console.WriteLine($"[RABBITMQ] Orden {order.OrderId} procesada exitosamente - {successfulUpdates.Count} productos actualizados");
        }
    }
}