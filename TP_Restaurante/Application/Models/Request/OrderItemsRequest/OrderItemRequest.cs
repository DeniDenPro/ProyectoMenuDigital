using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request.OrderItemsRequest
{
    public class OrderItemRequest
    {
        public int quantity { get; set; }
        public string? notes { get; set; }
    }
}
