using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.Areas.Identity.ViewModels
{
    public class MyOrdersViewModel
    {
        public int Id { get; set; }
        public string IdClient { get; set; }

        public string Date { get; set; }

        public string Address { get; set; }

        public string TotalPrice { get; set; }

        public string OrderStatus { get; set; }
    }
}
