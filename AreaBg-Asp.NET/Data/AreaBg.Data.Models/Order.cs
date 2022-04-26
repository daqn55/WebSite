using AreaBg.Data.Models.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AreaBg.Data.Models
{
    public class Order
    {
        public Order()
        {
            this.OrderBooks = new HashSet<OrderBook>();
        }

        public int Id { get; set; }

        public string OrderNumber { get; set; }
        public MyIdentityUser User { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DeliveryPlace DeliveryPlace { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public string Note { get; set; }
        public decimal TotalPriceWithoutDelivery { get; set; }
        public decimal DeliveryPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public DateTime OrderDate { get; set; }

        public ICollection<OrderBook> OrderBooks { get; set; }
    }
}
