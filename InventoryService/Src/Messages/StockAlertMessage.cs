using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Src.Shared.Messages
{
    public class StockAlertMessage
    {
        public string Sender { get; set; } = "InventoryService";
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductCategory { get; set; } = string.Empty;
        public int CurrentStock { get; set; }
        public int ThresholdLimit { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}