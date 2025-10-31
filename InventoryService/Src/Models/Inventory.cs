using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Src.Models
{
    /// <summary>
    /// Clase que representa el inventario de productos.
    /// </summary>
    public class Inventory
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
        /// Categoría del producto.
        /// </summary>
        public string ProductCategory { get; set; } = string.Empty;
        /// <summary>
        /// Cantidad en stock del producto.
        /// </summary>
        public int StockQuantity { get; set; }
        /// <summary>
        /// Estado del producto (disponible o no).
        /// </summary>
        public bool ProductStatus { get; set; } = true;
        /// <summary>
        /// Límite de umbral para el stock del producto.
        /// </summary>
        public int ThresholdLimit { get; set; } = 0;
        /// <summary>
        /// Fecha de creación del registro.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        /// <summary>
        /// Fecha de última actualización del registro.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}