using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.Areas.Admin.ViewModels.Orders
{
    public class OrderChangeQuantity
    {
        public BookQuantity[] Quantities { get; set; }

        public int OrderId { get; set; }
    }
}
