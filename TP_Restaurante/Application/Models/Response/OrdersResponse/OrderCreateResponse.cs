using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Response.OrdersResponse
{
    public class OrderCreateResponse
    {
        public long orderNumber { get; set; }
        public double totalAmount { get; set; }
        public DateTime createAt { get; set; }
    }
}
