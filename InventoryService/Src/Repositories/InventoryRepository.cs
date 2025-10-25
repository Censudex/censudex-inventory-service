using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryService.Src.Data;
using InventoryService.Src.Dtos;
using InventoryService.Src.Interface;
using InventoryService.Src.Mappings;
using InventoryService.Src.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Src.Repositories
{
    /// <summary>
    /// Repositorio para gestionar las operaciones de inventario.
    /// </summary>
    public class InventoryRepository : IInventoryRepository
    {
        /// <summary>
        /// Contexto de la base de datos.
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor del repositorio de inventario.
        /// </summary>
        /// <param name="context">Contexto de la base de datos.</param>
        public InventoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los elementos del inventario.
        /// </summary>
        /// <returns>Lista de elementos del inventario.</returns>
        public async Task<IEnumerable<Inventory>> GetAllInventoryItemsAsync()
        {
            return await _context.Inventory.ToListAsync();
        }

        /// <summary>
        /// Obtiene un elemento del inventario por su ID.
        /// </summary>
        /// <param name="id">Id del producto.</param>
        /// <returns>Elemento del inventario.</returns>
        /// <exception cref="KeyNotFoundException">El elemento del inventario no fue encontrado.</exception>
        public async Task<Inventory> GetInventoryEntityByIdAsync(Guid id)
        {
            var inventoryItem = await _context.Inventory.FindAsync(id) ?? throw new KeyNotFoundException("Inventory item not found.");
            return inventoryItem;
        }

        /// <summary>
        /// Obtiene un elemento del inventario como DTO por su ID.
        /// </summary>
        /// <param name="id">Id del producto.</param>
        /// <returns>Elemento del inventario como DTO.</returns>
        /// <exception cref="KeyNotFoundException">El elemento del inventario no fue encontrado.</exception>
        public async Task<ItemDto> GetInventoryItemDtoByIdAsync(Guid id)
        {
            var inventoryItem = await _context.Inventory.FindAsync(id) ?? throw new KeyNotFoundException("Inventory item not found.");
            return inventoryItem.ToItemDto();
        }

        /// <summary>
        /// Actualiza un elemento del inventario.
        /// </summary>
        /// <param name="item">Elemento del inventario.</param>
        /// <returns>Elemento del inventario actualizado.</returns>
        public async Task<Inventory> UpdateInventoryItem(Inventory item)
        {
            _context.Inventory.Update(item);
            await  _context.SaveChangesAsync();
            return item;
        }
    }
}