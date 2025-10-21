using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Src.Shared.Messages
{
    public class StockAlertMessage
    {
        public string? Sender { get; set; }
        public string? Text { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}