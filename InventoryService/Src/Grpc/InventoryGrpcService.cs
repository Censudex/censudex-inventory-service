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
    /// <summary>
    /// Servicio gRPC para la gestión del inventario.
    /// </summary>
    public class InventoryGrpcService : Protos.InventoryService.InventoryServiceBase
    {
        /// <summary>
        /// Repositorio de inventario.
        /// </summary>
        private readonly IInventoryRepository _inventoryRepository;
        /// <summary>
        /// Servicio de inventario.
        /// </summary>
        private readonly IInventoryService _inventoryService;

        /// <summary>
        /// Constructor del servicio gRPC de inventario.
        /// </summary>
        /// <param name="inventoryRepository">Repositorio de inventario.</param>
        /// <param name="inventoryService">Servicio de inventario.</param>
        public InventoryGrpcService(IInventoryRepository inventoryRepository, IInventoryService inventoryService)
        {
            _inventoryRepository = inventoryRepository;
            _inventoryService = inventoryService;
        }

        /// <summary>
        /// Obtiene todos los elementos del inventario.
        /// </summary>
        /// <param name="request">Request con los parámetros de la consulta.</param>
        /// <param name="context">Contexto de la llamada gRPC.</param>
        /// <returns>Response con la lista de elementos del inventario.</returns>
        /// <exception cref="RpcException">Error al obtener los elementos del inventario.</exception>
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
                        ProductStock = item.StockQuantity,
                        ProductStatus = item.ProductStatus,
                    });
                }

                return response;
            }
            catch (Exception)
            {
                throw new RpcException(new Status(StatusCode.Internal, "An error occurred while retrieving inventory items."));
            }
        }

        /// <summary>
        /// Obtiene un elemento del inventario por su ID.
        /// </summary>
        /// <param name="request">Request con el ID del elemento a obtener.</param>
        /// <param name="context">Contexto de la llamada gRPC.</param>
        /// <returns>Response con el elemento del inventario.</returns>
        /// <exception cref="RpcException">Error al obtener el elemento del inventario.</exception>
        public override async Task<GetInventoryItemByIdResponse> GetInventoryItemById(GetInventoryItemByIdRequest request, ServerCallContext context)
        {
            try
            {
                var item = await _inventoryRepository.GetInventoryItemDtoByIdAsync(Guid.Parse(request.ItemId));

                var response = new GetInventoryItemByIdResponse
                {
                    Item = new InventoryItemById
                    {
                        ProductId = item.ProductId.ToString(),
                        ProductName = item.ProductName,
                        ProductCategory = item.ProductCategory,
                        ProductStock = item.StockQuantity,
                        ProductStatus = item.ProductStatus,
                        ThresholdLimit = item.ThresholdLimit,
                        CreatedAt = item.CreatedAt.ToString("o"),
                        UpdatedAt = item.UpdatedAt?.ToString("o") ?? "",
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

        /// <summary>
        /// Actualiza el stock de un elemento del inventario.
        /// </summary>
        /// <param name="request">Request con los datos del elemento a actualizar.</param>
        /// <param name="context">Contexto de la llamada gRPC.</param>
        /// <returns>Response con el resultado de la operación de actualización.</returns>
        /// <exception cref="RpcException">Error al actualizar el elemento del inventario.</exception>
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

            }
            catch (RpcException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new RpcException(new Status(StatusCode.Internal, "An error occurred while updating the inventory item stock."));
            }
        }

        public override async Task<CreateInventoryItemResponse> CreateInventoryItem(CreateInventoryItemRequest request, ServerCallContext context)
        {
            try
            {
                var newItem = new Inventory
                {
                    ProductId = Guid.Parse(request.ProductId),
                    ProductName = request.ProductName,
                    ProductCategory = request.ProductCategory,
                    StockQuantity = request.ProductStock,
                    ProductStatus = request.ProductStatus,
                    ThresholdLimit = Random.Shared.Next(5, 30),
                    CreatedAt = DateTime.UtcNow
                };

                await _inventoryRepository.CreateInventoryItem(newItem);

                var response = new CreateInventoryItemResponse
                {
                    Success = true,
                    Message = "Inventory item created successfully.",
                    ProductId = newItem.ProductId.ToString(),
                    ProductName = newItem.ProductName,
                    ProductCategory = newItem.ProductCategory,
                    ProductStock = newItem.StockQuantity,
                    ProductStatus = newItem.ProductStatus,
                    ThresholdLimit = newItem.ThresholdLimit
                };

                return response;
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, $"An error occurred while creating the inventory item: {ex.Message}"));
            }
        }
    }
}