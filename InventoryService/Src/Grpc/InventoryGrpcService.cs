using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using InventoryService.Protos;
using InventoryService.Src.Dtos;
using InventoryService.Src.Interface;
using InventoryService.Src.Models;

namespace InventoryService.Src.Grpc
{
    public class InventoryGrpcService : Protos.InventoryService.InventoryServiceBase
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IInventoryService _inventoryService;

        public InventoryGrpcService(IInventoryRepository inventoryRepository, IInventoryService inventoryService)
        {
            _inventoryRepository = inventoryRepository;
            _inventoryService = inventoryService;
        }

        public override async Task<GetAllInventoryItemsResponse> GetAllInventoryItems(GetAllInventoryItemsRequest request, ServerCallContext context)
        {
            try
            {
                var items = await _inventoryRepository.GetAllInventoryItemsAsync();
                var response = new GetAllInventoryItemsResponse();

                foreach (var item in items)
                {
                    response.Items.Add(new InventoryItem
                    {
                        ProductId = item.ProductId.ToString(),
                        ProductName = item.ProductName,
                        ProductCategory = item.ProductCategory,
                        ProductStock = item.StockQuantity,
                    });
                }

                return response;
            }
            catch (Exception)
            {
                throw new RpcException(new Status(StatusCode.Internal, "An error occurred while retrieving inventory items."));
            }
        }

        public override async Task<GetInventoryItemByIdResponse> GetInventoryItemById(GetInventoryItemByIdRequest request, ServerCallContext context)
        {
            try
            {
                var item = await _inventoryRepository.GetInventoryItemDtoByIdAsync(Guid.Parse(request.ItemId));

                var response = new GetInventoryItemByIdResponse
                {
                    Item = new InventoryItem
                    {
                        ProductId = item.ProductId.ToString(),
                        ProductName = item.ProductName,
                        ProductCategory = item.ProductCategory,
                        ProductStock = item.StockQuantity,
                    }
                };

                return response;
            }
            catch (KeyNotFoundException)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Inventory item not found."));
            }
            catch (Exception)
            {
                throw new RpcException(new Status(StatusCode.Internal, "An error occurred while retrieving the inventory item."));
            }
        }

        public override async Task<UpdateInventoryItemStockResponse> UpdateInventoryItemStock(UpdateInventoryItemStockRequest request, ServerCallContext context)
        {
            try
            {
                if (!Guid.TryParse(request.ItemId, out var itemId))
                {
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid ItemId format."));
                }

                var updateStockDto = new UpdateStockDto
                {
                    Operation = request.Operation,
                    Quantity = request.Quantity
                };

                var result = await _inventoryService.UpdateInventoryItemStockAsync(itemId, updateStockDto);

                var response = new UpdateInventoryItemStockResponse
                {
                    Item = new UpdateOperation
                    {
                        Success = result.Success,
                        Message = result.Message,
                        ProductId = itemId.ToString(),
                        ProductName = result.ProductName,
                        ProductCategory = result.ProductCategory,
                        PreviousStock = result.PreviousStock,
                        UpdatedStock = result.UpdatedStock,
                        Operation = result.Operation,
                        QuantityChanged = result.QuantityChanged,
                        Alert = result.Alert ?? ""
                    }
                };

                return response;
                
            } catch (RpcException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new RpcException(new Status(StatusCode.Internal, "An error occurred while updating the inventory item stock."));
            }
        }
    }
}