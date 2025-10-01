using Application.Exceptions;
using Application.Interfaces.IOrder;
using Application.Interfaces.IOrder.IOrderService;
using Application.Interfaces.IStatus;
using Application.Models.Response;
using Application.Models.Response.DishesResponse;
using Application.Models.Response.OrdersResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.OrderService
{
    public class GetOrderFechaStatusService : IGetOrderFechaStatusService
    {
        private readonly IOrderQuery _orderQuery;
        private readonly IOrderCommand _orderCommand;
        private readonly IStatusQuery _statusQuery;
        public GetOrderFechaStatusService(IOrderQuery orderQuery, IOrderCommand orderCommand, IStatusQuery statusQuery)
        {
            _orderQuery = orderQuery;
            _orderCommand = orderCommand;
            _statusQuery = statusQuery;
        }
        public async Task<IEnumerable<OrderDetailsResponse?>> GetOrderFechaStatus(DateTime? from, DateTime? to, int? statusid)
        {
            // Validación de Rango de Fechas
            if (from.HasValue && to.HasValue && from.Value > to.Value)
            {
                throw new BadRequestException("Rango de fechas inválido: la fecha 'desde' no puede ser posterior a la fecha 'hasta'.");
            }
            // Validación de existencia del Status
            if (statusid.HasValue)
            {
                var statusExists = await _statusQuery.StatusExists(statusid.Value);
                if (!statusExists)
                {
                    throw new BadRequestException($"El estado con ID {statusid.Value} no es válido.");
                }
            }
            var orders = await _orderQuery.GetOrderFechaStatus(from, to, statusid);
            var orderResponses = orders.Select(o => new OrderDetailsResponse
            {
                OrderNumber = (int)o.OrderId,
                TotalAmount = (double)o.Price,
                DeliveryTo = o.DeliveryTo,
                Notes = o.Notes,
                Status = new GenericResponse { Id = o.StatusId, Name = o.OverallStatus.Name },
                DeliveryType = new GenericResponse { Id = o.DeliveryTypeId, Name = o.DeliveryType.Name },
                Items = o.OrderItems.Select(oi => new OrderItemResponse
                {
                    Id = oi.OrderItemId,
                    Quantity = oi.Quantity,
                    Notes = oi.Notes,
                    Status = new GenericResponse { Id = oi.StatusId, Name = oi.Status.Name },
                    Dish = new DishShortResponse
                    {
                        Id = oi.DishId,
                        Name = oi.Dish.Name,
                        Image = oi.Dish.ImageUrl
                    }

                }).ToList(),
                CreatedAt = o.CreateDate,
                UpdatedAt = o.UpdateDate
            });
            return orderResponses;
        }
    }
}
