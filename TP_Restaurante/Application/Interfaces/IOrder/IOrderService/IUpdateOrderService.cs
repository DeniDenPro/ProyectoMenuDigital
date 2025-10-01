using Application.Models.Request.OrdersRequest;
using Application.Models.Response.OrdersResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IOrder.IOrderService
{
    public interface IUpdateOrderService
    {
        Task<OrderUpdateResponse> UpdateOrder(long orderId, OrderUpdateRequest ItemRequest);
    }
}
