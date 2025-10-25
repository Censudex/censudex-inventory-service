using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Src.Messages
{
    /// <summary>
    /// Detalle de fallo de stock para un producto específico.
    /// </summary>
    public class StockFailure
    {
        /// <summary>
        /// Identificador único del producto.
        /// </summary>
        public Guid ProductId { get; set; }
        /// <summary>
        /// Nombre del producto.
        /// </summary>
        public string ProductName { get; set; } = string.Empty;
        /// <summary>
        /// Cantidad solicitada del producto.
        /// </summary>
        public int RequestedQuantity { get; set; }
        /// <summary>
        /// Cantidad disponible en stock.
        /// </summary>
        public int AvailableStock { get; set; }
    }
}