using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryService.Src.Messages;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Src.Simulations
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderServiceSimulator : ControllerBase
    {
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
                        ProductId = Guid.Parse("019a091e-f285-7167-ada4-0c1ba7772cef"),
                        Quantity = 2
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
                        ProductId = Guid.Parse("019a091e-f285-7167-ada4-0c1ba7772cef"),
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