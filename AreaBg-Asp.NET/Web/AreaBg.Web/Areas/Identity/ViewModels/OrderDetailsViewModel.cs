using AreaBg.Data.Models.enums;
using AreaBg.Web.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.Areas.Identity.ViewModels
{
    public class OrderDetailsViewModel
    {
        public OrderDetailsViewModel()
        {
            this.OrderBooks = new HashSet<ProductCartDetails>();
        }

        public int Id { get; set; }

        public string OrderStatus { get; set; }

        public string Email { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public string DeliveryPlace { get; set; }

        public string Note { get; set; }
        public decimal TotalPriceWithoutDelivery { get; set; }
        public decimal DelieryPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public DateTime OrderDate { get; set; }

        public ICollection<ProductCartDetails> OrderBooks { get; set; }
    }
}
