using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Src.Shared.Messages
{
    /// <summary>
    /// Mensaje que representa una alerta de stock bajo.
    /// </summary>
    public class StockAlertMessage
    {
        /// <summary>
        /// Nombre del servicio que envía la alerta.
        /// </summary>
        public string Sender { get; set; } = "InventoryService";
        /// <summary>
        /// Identificador único del producto.
        /// </summary>
        public Guid ProductId { get; set; }
        /// <summary>
        /// Nombre del producto.
        /// </summary>
        public string ProductName { get; set; } = string.Empty;
        /// <summary>
        /// Categoría del producto.
        /// </summary>
        public string ProductCategory { get; set; } = string.Empty;
        /// <summary>
        /// Cantidad actual en stock.
        /// </summary>
        public int CurrentStock { get; set; }
        /// <summary>
        /// Límite de umbral para la alerta.
        /// </summary>
        public int ThresholdLimit { get; set; }
        /// <summary>
        /// Texto descriptivo de la alerta.
        /// </summary>
        public string Text { get; set; } = string.Empty;
        /// <summary>
        /// Marca de tiempo de cuando se generó la alerta.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}