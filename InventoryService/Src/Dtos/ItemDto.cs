using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Src.Dtos
{
    public class ItemDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductCategory { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public bool ProductStatus { get; set; } = true;
        public int ThresholdLimit { get; set; } = 0;
    }
}