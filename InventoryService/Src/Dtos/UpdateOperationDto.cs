using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Src.Dtos
{
    /// <summary>
    /// Clase DTO para representar el resultado de una operación de actualización de inventario.
    /// </summary>
    public class UpdateOperationDto
    {
        /// <summary>
        /// Indica si la operación fue exitosa.
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Mensaje descriptivo de la operación.
        /// </summary>
        public string Message { get; set; } = string.Empty;
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
        /// Cantidad de stock antes de la operación.
        /// </summary>
        public int PreviousStock { get; set; }
        /// <summary>
        /// Cantidad de stock después de la operación.
        /// </summary>
        public int UpdatedStock { get; set; }
        /// <summary>
        /// Tipo de operación realizada (ej. "añadir", "quitar").
        /// </summary>
        public string Operation { get; set; } = string.Empty;
        /// <summary>
        /// Cantidad de producto añadida o quitada.
        /// </summary>
        public int QuantityChanged { get; set; }
        /// <summary>
        /// Mensaje de alerta en caso de que se alcance el límite de umbral.
        /// </summary>
        public string? Alert { get; set; }
    }
}