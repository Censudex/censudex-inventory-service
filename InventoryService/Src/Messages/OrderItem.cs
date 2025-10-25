using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Src.Messages
{
    /// <summary>
    /// Artículo de un pedido.
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// Identificador único del producto.
        /// </summary>
        public Guid ProductId { get; set; }
        /// <summary>
        /// Cantidad del producto en el pedido.
        /// </summary>
        public int Quantity { get; set; }
    }
}