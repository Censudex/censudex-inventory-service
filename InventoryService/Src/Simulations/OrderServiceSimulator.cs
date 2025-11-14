using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryService.Src.Messages;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Src.Simulations
{
    /// <summary>
    /// Simula la creación de órdenes para pruebas.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OrderServiceSimulator : ControllerBase
    {
        /// <summary>
        /// Simula la creación de una orden publicando un evento de "OrderCreated".
        /// </summary>
        /// <param name="publishEndpoint">Endpoint de publicación para mensajes.</param>
        /// <returns>Resultado de la simulación.</returns>
        [HttpPost("test/simulate-order")]
        public async Task<ActionResult> SimulateOrder([FromServices] IPublishEndpoint publishEndpoint)
        {
            var testOrder = new OrderCreatedMessage
            {
                OrderId = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductId = Guid.Parse("019a83e7-ca65-7a1e-a1ce-4b72d0c8ea27"),
                        Quantity = 90
                    }
                },
                CreatedAt = DateTime.UtcNow
            };

            await publishEndpoint.Publish(testOrder);

            return Ok(new
            {
                message = "Test order created event published",
                orderId = testOrder.OrderId
            });
        }
        
        [HttpPost("test/simulate-order-fail")]
        public async Task<ActionResult> SimulateOrderFail([FromServices] IPublishEndpoint publishEndpoint)
        {
            var testOrder = new OrderCreatedMessage
            {
                OrderId = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                Items = new List<OrderItem>
                {
                    new OrderItem 
                    { 
                        ProductId = Guid.Parse("019a83e7-ca65-7a1e-a1ce-4b72d0c8ea27"),
                        Quantity = 1000 // ← Cantidad mayor al stock disponible
                    }
                },
                CreatedAt = DateTime.UtcNow
            };

            await publishEndpoint.Publish(testOrder);

            return Ok(new { message = "Test order with insufficient stock published" });
        }
    }
}