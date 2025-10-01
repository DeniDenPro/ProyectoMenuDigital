using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order
    {
        public long OrderId { get; set; }
        public string? DeliveryTo { get; set; }
        public string Notes { get; set; }
        public decimal Price { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        //FK DeliveryType
        public int DeliveryTypeId { get; set; }
        public DeliveryType DeliveryType { get; set; }

        //FK Status
        public int StatusId { get; set; }
        public Status OverallStatus { get; set; }

        //Relacion OrderItem
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
