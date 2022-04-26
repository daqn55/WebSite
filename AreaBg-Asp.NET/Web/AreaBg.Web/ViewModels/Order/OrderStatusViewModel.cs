using AreaBg.Web.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.ViewModels.Order
{
    public class OrderStatusViewModel
    {
        public string OrderId { get; set; }

        public string Status { get; set; }

        public string TotalPrice { get; set; }

        public string TotalPriceWithoutDelivery { get; set; }

        public string DeliveryPrice { get; set; }

        public List<ProductStatusViewModel> Books { get; set; }
    }
}
