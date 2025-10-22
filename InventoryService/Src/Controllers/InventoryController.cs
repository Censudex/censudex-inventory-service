using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryService.Src.Dtos;
using InventoryService.Src.Interface;
using InventoryService.Src.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryRepository inventoryRepository, IInventoryService inventoryService)
        {
            _inventoryRepository = inventoryRepository;
            _inventoryService = inventoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInventoryItems()
        {
            try
            {
                var items = await _inventoryRepository.GetAllInventoryItemsAsync();
                var response = new ApiResponse<IEnumerable<Inventory>>
                {
                    Success = true,
                    Message = "Inventory items retrieved successfully",
                    Data = items
                };
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving inventory items." });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInventoryItemById(Guid id)
        {
            try
            {
                var item = await _inventoryRepository.GetInventoryItemDtoByIdAsync(id);
                var response = new ApiResponse<ItemDto>
                {
                    Success = true,
                    Message = "Inventory item retrieved successfully",
                    Data = item
                };
                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Inventory item not found." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the inventory item." });
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateInventoryItemStock(Guid id, [FromBody] UpdateStockDto updateStockDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (updateStockDto.Quantity <= 0)
            {
                return BadRequest(new { message = "Invalid quantity. Quantity must be greater than zero." });
            }
            
            try
            {
                var updateResult = await _inventoryService.UpdateInventoryItemStockAsync(id, updateStockDto);
                
                if (!updateResult.Success)
                {
                    throw new InvalidOperationException(updateResult.Message);
                }

                var response = new ApiResponse<UpdateOperationDto>
                {
                    Success = true,
                    Message = "Inventory item stock updated successfully",
                    Data = updateResult
                };
                
                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Inventory item not found." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while updating the inventory item stock." });
            }
        }
    }
}