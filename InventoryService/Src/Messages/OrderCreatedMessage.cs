using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Src.Messages
{
    public class OrderCreatedMessage
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }
}