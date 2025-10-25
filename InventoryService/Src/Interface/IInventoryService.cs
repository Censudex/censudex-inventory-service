using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryService.Src.Dtos;

namespace InventoryService.Src.Interface
{
    /// <summary>
    /// Interfaz para el servicio de inventario.
    /// </summary>
    public interface IInventoryService
    {
        /// <summary>
        /// Actualiza el stock de un elemento de inventario.
        /// </summary>
        /// <param name="id">ID del elemento de inventario.</param>
        /// <param name="updateStockDto">Datos de actualización de stock.</param>
        /// <returns> Resultado de la operación de actualización.</returns>
        public Task<UpdateOperationDto> UpdateInventoryItemStockAsync(Guid id, UpdateStockDto updateStockDto);
    }
}