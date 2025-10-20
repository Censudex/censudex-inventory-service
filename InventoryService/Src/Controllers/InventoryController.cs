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

        public InventoryController(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
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
    }
}