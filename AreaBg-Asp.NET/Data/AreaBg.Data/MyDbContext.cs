using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AreaBg.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AreaBg.Data
{
    public class MyDbContext : IdentityDbContext<MyIdentityUser>
    {
        public DbSet<Book> Books { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Subcategory> Subcategories { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderBook> OrderBooks { get; set; }

        public DbSet<UserAddress> UserAddresses { get; set; }

        public DbSet<UserOrder> UserOrders { get; set; }

        public DbSet<UserFavoriteProduct> UserFavoriteProducts { get; set; }

        public DbSet<SiteEmail> SiteEmails { get; set; }

        public DbSet<HelpAndTerms> HelpAndTerms { get; set; }

        public DbSet<BookReview> BookReviews { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
