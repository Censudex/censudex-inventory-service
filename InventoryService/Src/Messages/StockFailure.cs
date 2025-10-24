using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Src.Messages
{
    public class StockFailure
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int RequestedQuantity { get; set; }
        public int AvailableStock { get; set; }
    }
}