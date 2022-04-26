using AreaBg.Data.Models.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AreaBg.Data.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Note { get; set; }
        public DeliveryPlace DeliveryPlace { get; set; }
    }
}
