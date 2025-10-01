using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Response.OrdersResponse
{
    public class OrderDetailsResponse
    {
        public long OrderNumber { get; set; }
        public double TotalAmount { get; set; }
        public string? DeliveryTo { get; set; }
        public string? Notes { get; set; }

        public GenericResponse Status { get; set; }
        public GenericResponse DeliveryType { get; set; }

        public List<OrderItemResponse>? Items { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
