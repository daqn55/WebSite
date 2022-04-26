using AreaBg.Data.Models.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.Areas.Identity.ViewModels
{
    public class MyAddressViewModel
    {
        public int Id { get; set; }

        public string City { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Name { get; set; }

        public DeliveryPlace DeliveryPlace { get; set; }

        public string Note { get; set; }
    }
}
