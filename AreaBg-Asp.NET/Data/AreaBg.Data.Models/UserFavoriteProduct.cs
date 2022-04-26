using System;
using System.Collections.Generic;
using System.Text;

namespace AreaBg.Data.Models
{
    public class UserFavoriteProduct
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public MyIdentityUser User { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
