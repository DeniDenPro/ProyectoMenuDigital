using Application.Exceptions;
using Application.Interfaces.IDeliveryType.IDeliveryTypeService;
using Application.Interfaces.IOrder;
using Application.Interfaces.IOrder.IOrderService;
using Application.Models.Request;
using Application.Models.Response;
using Application.Models.Response.DishesResponse;
using Application.Models.Response.OrdersResponse;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP_Restaurante.Exceptions;

namespace Application.Services.OrderService
{
    public class GetOrderByIdService : IGetOrdersByIdService
    {
        private readonly IOrderQuery _orderQuery;
        public GetOrderByIdService(IOrderQuery orderQuery)
        {
            _orderQuery = orderQuery;
        }
        public async Task<OrderDetailsResponse> GetOrderById(long orderId)
        {
            var order = await _orderQuery.GetFullOrderById(orderId);
            if(order != null)
            {
                var orderDetails = new OrderDetailsResponse
                {
                    OrderNumber = (int)order.OrderId,
                    TotalAmount = (double)order.Price,
                    DeliveryTo = order.DeliveryTo,
                    Notes = order.Notes,
                    Status = new GenericResponse { Id = order.StatusId, Name = order.OverallStatus?.Name ?? "Desconocido" },
                    DeliveryType = new GenericResponse { Id = order.DeliveryTypeId, Name = order.DeliveryType?.Name ?? "Desconocido" },
                    Items = order.OrderItems.Select(item => new OrderItemResponse
                    {
                        Id = 2,
                        Quantity = item.Quantity,
                        Notes = item.Dish?.Name,
                        Dish = new DishShortResponse { Id = item.DishId, Name = item.Dish?.Name ?? "Desconocido", Image = item.Dish?.ImageUrl ?? "No encontrada" },
                        Status = new GenericResponse { Id = item.Status.Id, Name = item.Status?.Name ?? "Desconocido" }
                    }).ToList(),
                    CreatedAt = order.CreateDate,
                    UpdatedAt = order.UpdateDate
                };
                return orderDetails;
            };
            return null;
        }
    }
}
