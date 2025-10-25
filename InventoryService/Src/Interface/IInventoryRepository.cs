using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryService.Src.Dtos;
using InventoryService.Src.Models;

namespace InventoryService.Src.Interface
{
    /// <summary>
    /// Interfaz para el repositorio de inventario.
    /// </summary>
    public interface IInventoryRepository
    {
        /// <summary>
        /// Obtiene todos los elementos de inventario.
        /// </summary>
        /// <returns>Lista de elementos de inventario.</returns>
        public Task<IEnumerable<Inventory>> GetAllInventoryItemsAsync();

        /// <summary>
        /// Obtiene un DTO de elemento de inventario por su ID.
        /// </summary>
        /// <param name="id">Id del elemento de inventario.</param>
        /// <returns>DTO del elemento de inventario.</returns>
        public Task<ItemDto> GetInventoryItemDtoByIdAsync(Guid id);

        /// <summary>
        /// Obtiene una entidad de inventario por su ID.
        /// </summary>
        /// <param name="id">Id del elemento de inventario.</param>
        /// <returns>Entidad de inventario.</returns>
        public Task<Inventory> GetInventoryEntityByIdAsync(Guid id);

        /// <summary>
        /// Actualiza un elemento de inventario.
        /// </summary>
        /// <param name="item">Entidad de inventario a actualizar.</param>
        /// <returns>Entidad de inventario actualizada.</returns>
        public Task<Inventory> UpdateInventoryItem(Inventory item);
    }
}