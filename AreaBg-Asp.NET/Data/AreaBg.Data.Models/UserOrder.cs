using System;
using System.Collections.Generic;
using System.Text;

namespace AreaBg.Data.Models
{
    public class UserOrder
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public string UserId { get; set; }
        public MyIdentityUser User { get; set; }
    }
}
