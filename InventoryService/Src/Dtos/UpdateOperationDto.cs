using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Src.Dtos
{
    public class UpdateOperationDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductCategory { get; set; } = string.Empty;
        public int PreviousStock { get; set; }
        public int UpdatedStock { get; set; }
        public string Operation { get; set; } = string.Empty;
        public int QuantityChanged { get; set; }
    }
}