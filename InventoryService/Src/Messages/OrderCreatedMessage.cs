using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Src.Messages
{
    /// <summary>
    /// Mensaje que representa un pedido que ha sido creado.
    /// </summary>
    public class OrderCreatedMessage
    {
        /// <summary>
        /// Identificador único del pedido.
        /// </summary>
        public Guid OrderId { get; set; }
        /// <summary>
        /// Identificador único del cliente.
        /// </summary>
        public Guid CustomerId { get; set; }
        /// <summary>
        /// Lista de artículos en el pedido.
        /// </summary>
        public List<OrderItem> Items { get; set; } = new();
        /// <summary>
        /// Fecha y hora en que se creó el pedido.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}