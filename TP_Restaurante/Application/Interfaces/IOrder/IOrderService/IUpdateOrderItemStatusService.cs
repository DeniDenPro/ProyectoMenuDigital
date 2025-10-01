using Application.Models.Request.OrderItemsRequest;
using Application.Models.Response.OrdersResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IOrder.IOrderService
{
    public interface IUpdateOrderItemStatusService
    {
        Task<OrderUpdateResponse> UpdateStatus(long orderId, int itemId, OrderItemUpdateRequest request);
    }
}
