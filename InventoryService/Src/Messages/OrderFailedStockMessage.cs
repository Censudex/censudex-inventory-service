using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Src.Messages
{
    /// <summary>
    /// Mensaje que representa un pedido que ha fallado por falta de stock.
    /// </summary>
    public class OrderFailedStockMessage
    {
        /// <summary>
        /// Identificador único del pedido.
        /// </summary>
        public Guid OrderId { get; set; }
        /// <summary>
        /// Motivo del fallo en el pedido.
        /// </summary>
        public string Reason { get; set; } = string.Empty;
        /// <summary>
        /// Lista de productos que no tienen stock suficiente.
        /// </summary>
        public List<StockFailure> FailedProducts { get; set; } = new();
        /// <summary>
        /// Fecha y hora en que falló el pedido.
        /// </summary>
        public DateTime FailedAt { get; set; } = DateTime.UtcNow;
    }
}