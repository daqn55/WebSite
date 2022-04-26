using AreaBg.Web.Areas.Identity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.Areas.Admin.ViewModels.Orders
{
    public class PrintOrdersViewModel
    {
        public List<OrderDetailsViewModel> OrderDetails { get; set; }

        public int OrdersCount { get; set; }

        public decimal OrdersTotalPrice { get; set; }

        public decimal OrderDeliveryPrices { get; set; }
    }
}
