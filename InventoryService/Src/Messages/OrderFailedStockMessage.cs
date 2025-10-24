using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Src.Messages
{
    public class OrderFailedStockMessage
    {
        public Guid OrderId { get; set; }
        public string Reason { get; set; } = string.Empty;
        public List<StockFailure> FailedProducts { get; set; } = new();
        public DateTime FailedAt { get; set; } = DateTime.UtcNow;
    }
}