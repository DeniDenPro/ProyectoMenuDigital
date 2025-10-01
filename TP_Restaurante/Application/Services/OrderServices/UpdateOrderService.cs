using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.IDish;
using Application.Interfaces.IOrder;
using Application.Interfaces.IOrder.IOrderService;
using Application.Interfaces.IOrderItem;
using Application.Models.Request;
using Application.Models.Request.OrdersRequest;
using Application.Models.Response.OrdersResponse;
using Azure.Core;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP_Restaurante.Exceptions;

namespace Application.Services.OrderService
{
    public class UpdateOrderService : IUpdateOrderService
    {
        private readonly IOrderQuery _orderQuery;
        private readonly IOrderCommand _orderCommand;
        private readonly IOrderItemQuery _orderItemQuery;
        private readonly IOrderItemCommand _orderItemCommand;
        private readonly IDishQuery _dishQuery;

        public UpdateOrderService(IOrderQuery orderQuery, IOrderCommand orderCommand, IOrderItemQuery orderItemQuery, IOrderItemCommand orderItemCommand, IDishQuery dishQuery)
        {
            _orderQuery = orderQuery;
            _orderCommand = orderCommand;
            _orderItemQuery = orderItemQuery;
            _orderItemCommand = orderItemCommand;
            _dishQuery = dishQuery;
        }
        public async Task<OrderUpdateResponse> UpdateOrder(long orderId, OrderUpdateRequest ItemRequest)
        {
            var order = await _orderQuery.GetOrderById(orderId); 
            if (order == null)
            {
                throw new NotFoundException($"La orden con ID {orderId} no existe.");
            }
            if (order.StatusId >= (int)StatusOrderEnum.InProgress)
            {
                throw new BadRequestException("No se puede modificar una orden que ya está en preparación.");
            }
           
            if (ItemRequest.Items == null || !ItemRequest.Items.Any())
                throw new BadRequestException("La orden debe contener al menos un ítem.");
            if (ItemRequest.Items.Any(item => item.quantity <= 0))
                throw new BadRequestException("La cantidad de cada ítem debe ser mayor a 0.");

            var dishIds = ItemRequest.Items.Select(i => i.id).ToList();
            var dishesFromDb = await _dishQuery.GetDishesByIds(dishIds);

            if (dishesFromDb.Count != dishIds.Count)
                throw new BadRequestException("Uno o más platos especificados no existen.");
            if (dishesFromDb.Any(d => !d.Available))
                throw new BadRequestException("Uno o más platos especificados no están disponibles.");

            // crear la nueva lista de items

            var newOrderItems = ItemRequest.Items.Select(item => new OrderItem
            {
                OrderId = orderId,
                DishId = item.id,
                Quantity = item.quantity,
                Notes = item.notes,
                StatusId = 1
            }).ToList();

            await _orderItemCommand.InsertOrderItemRange(newOrderItems);

            // Recalcular el precio total de la orden
            decimal newTotalPrice = 0;
            foreach (var item in newOrderItems)
            {
                var dish = await _dishQuery.GetDishById(item.DishId);
                newTotalPrice += dish.Price * item.Quantity;
            }

            // actualizar la orden principal
            order.Price += await CalculateTotalPrice(newOrderItems, dishesFromDb);
            order.UpdateDate = DateTime.Now;
            await _orderCommand.UpdateOrder(order);

            return new OrderUpdateResponse
            {
                OrderNumber = (int)order.OrderId,
                TotalAmount = (double)order.Price,
                UpdatedAt = order.UpdateDate 
            };
        }

        private async Task<decimal> CalculateTotalPrice(List<OrderItem> newItems, List<Dish> dishes)
        {
            var dishDictionary = dishes.ToDictionary(d => d.DishId);
            decimal total = 0;

            foreach (var item in newItems)
            {
                if (dishDictionary.TryGetValue(item.DishId, out var dish))
                {
                    total += dish.Price * item.Quantity;
                }
            }
            return total;
        }
    }
}
