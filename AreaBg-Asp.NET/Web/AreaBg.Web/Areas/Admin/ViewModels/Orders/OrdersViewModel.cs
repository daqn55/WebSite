using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.Areas.Admin.ViewModels.Orders
{
    public class OrdersViewModel
    {
        public int Id { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string Date { get; set; }

        public string City { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }

        public int OrderStatus { get; set; }

        public string Email { get; set; }

        public bool Registred { get; set; }
    }
}
