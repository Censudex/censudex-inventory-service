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
                var updateStockDto = new UpdateStockDto
                {
                    Operation = request.Operation,
                    Quantity = request.Quantity
                };

                var updateResult = await _inventoryService.UpdateInventoryItemStockAsync(Guid.Parse(request.ItemId), updateStockDto);

                var response = new UpdateInventoryItemStockResponse
                {
                    Item = new UpdateOperation
                    {
                        ProductId = updateResult.ProductId.ToString(),
                        ProductName = updateResult.ProductName,
                        ProductCategory = updateResult.ProductCategory,
                        PreviousStock = updateResult.PreviousStock,
                        UpdatedStock = updateResult.UpdatedStock,
                        Operation = updateResult.Operation,
                        QuantityChanged = updateResult.QuantityChanged,
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
                throw new RpcException(new Status(StatusCode.Internal, "An error occurred while updating the inventory item."));
            }   
        }
    }
}