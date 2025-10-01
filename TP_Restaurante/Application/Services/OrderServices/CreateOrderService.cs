using Application.Interfaces.IDeliveryType;
using Application.Interfaces.IDish;
using Application.Interfaces.IOrder;
using Application.Interfaces.IOrder.IOrderService;
using Application.Interfaces.IOrderItem;
using Application.Models.Request;
using Application.Models.Request.OrdersRequest;
using Application.Models.Response.OrdersResponse;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP_Restaurante.Exceptions;

namespace Application.Services.OrderServices
{
    public class CreateOrderService : ICreateOrderService
    {
        private readonly IOrderCommand _orderCommand;
        private readonly IDeliveryTypeQuery _deliveryTypeQuery;
        private readonly IDishQuery _dishQuery;
        private readonly IOrderItemCommand _orderItemCommand;
        public CreateOrderService(IOrderCommand orderCommand, IDeliveryTypeQuery deliveryTypeQuery, IDishQuery dishQuery, IOrderItemCommand orderItemCommand)
        {
            _orderCommand = orderCommand;
            _deliveryTypeQuery = deliveryTypeQuery;
            _dishQuery = dishQuery;
            _orderItemCommand = orderItemCommand;
        }

        public async Task<OrderCreateResponse?> CreateOrder(OrderRequest orderRequest)
        {

            var deliveryType = await _deliveryTypeQuery.GetDeliveryTypeById(orderRequest.delivery.id);
            if (deliveryType == null)
            {
                throw new NotFoundException("Tipo de entrega no existe");
            }

            var order = new Order
            {
                DeliveryTypeId = orderRequest.delivery.id,
                Price = 0,
                StatusId = 1,
                DeliveryTo = orderRequest.delivery.to,
                Notes = orderRequest.notes,
                UpdateDate = DateTime.Now,
                CreateDate = DateTime.Now
            };
            await _orderCommand.InsertOrder(order);
            var listItems = orderRequest.items;
            var listorderItems = listItems.Select(item => new OrderItem
            {
                DishId = item.id,
                Quantity = item.quantity,
                Notes = item.notes,
                StatusId = 1,
                OrderId = order.OrderId,
            }).ToList();
            order.Price = await CalculateTotalPrice(listItems);
            await _orderItemCommand.InsertOrderItemRange(listorderItems);
            await _orderCommand.UpdateOrder(order);

            return new OrderCreateResponse
            {
                orderNumber = (int)order.OrderId,
                totalAmount = (double)order.Price,
                createAt = DateTime.Now
            };
        }

        private async Task<decimal> CalculateTotalPrice(List<Items> orderItems)
        {
            decimal total = 0;
            //dish obtener
            foreach (var item in orderItems)
            {
                var dish = await _dishQuery.GetDishById(item.id);
                total += dish.Price * item.quantity;
            }
            return total;
        }
    }
    
}
