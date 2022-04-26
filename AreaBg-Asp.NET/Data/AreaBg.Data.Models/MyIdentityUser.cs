using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace AreaBg.Data.Models
{
    // Add profile data for application users by adding properties to the MyIdentityUser class
    public class MyIdentityUser : IdentityUser
    {
        public MyIdentityUser()
        {
            this.UserOrders = new HashSet<UserOrder>();
            this.UserAddresses = new HashSet<UserAddress>();
            this.UserFavoriteProducts = new HashSet<UserFavoriteProduct>();
        }

        public string FirstName { get; set; }

        public bool AcceptTerms { get; set; }

        public ICollection<UserOrder> UserOrders { get; set; }

        public ICollection<UserAddress> UserAddresses { get; set; }

        public ICollection<UserFavoriteProduct> UserFavoriteProducts { get; set; }
    }
}
