using System;
using System.Collections.Generic;
using System.Text;

namespace AreaBg.Data.Models
{
    public class UserAddress
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public MyIdentityUser User { get; set; }

        public int AddressId { get; set; }
        public Address Address { get; set; }
    }
}
