using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Src.Dtos
{
    /// <summary>
    /// Clase DTO para representar una solicitud de actualización de stock.
    /// </summary>
    public class UpdateStockDto
    {
        /// <summary>
        /// Tipo de operación a realizar: "increase" para aumentar el stock, "decrease" para disminuirlo.
        /// </summary>
        [RegularExpression("^(?i)(increase|decrease)$", ErrorMessage = "Operation must be either 'increase' or 'decrease'.")]
        public required string Operation { get; set; } = string.Empty;

        /// <summary>
        /// Cantidad de producto a añadir o quitar.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive integer.")]
        public required int Quantity { get; set; }
    }
}