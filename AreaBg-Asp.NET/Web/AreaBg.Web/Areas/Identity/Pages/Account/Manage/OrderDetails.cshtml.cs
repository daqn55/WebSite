using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AreaBg.Data;
using AreaBg.Data.Models;
using AreaBg.Web.Areas.Identity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AreaBg.Web.GlobalsVariables;

namespace AreaBg.Web.Areas.Identity.Pages.Account.Manage
{
    public class OrderDetailsModel : PageModel
    {
        private readonly MyDbContext Db;
        private readonly UserManager<MyIdentityUser> UserManager;

        public OrderDetailsModel(MyDbContext db, UserManager<MyIdentityUser> userManager)
        {
            this.Db = db;
            this.UserManager = userManager;
        }

        [BindProperty]
        public OrderDetailsViewModel Order { get; set; }

        public IActionResult OnGet(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/account/login?returnUrl=" + HttpContext.Request.Path);
            }

            var userId = this.UserManager.GetUserId(User);
            var deliveryPlace = Globals.DeliveryPlaces[(int)this.Db.Orders.Find(id).DeliveryPlace];
            var orderStatus = Globals.OrderStatus[(int)this.Db.Orders.Find(id).OrderStatus];

            this.Order = this.Db.UserOrders
                                    .Include(x => x.Order)
                                    .ThenInclude(x => x.OrderBooks)
                                    .Where(x => x.UserId == userId && x.OrderId == id)
                                    .Select(x => new OrderDetailsViewModel
                                    {
                                        Id = x.OrderId,
                                        Address = x.Order.Address,
                                        City = x.Order.City,
                                        DeliveryPlace = deliveryPlace,
                                        Name = x.Order.Name,
                                        Note = x.Order.Note,
                                        OrderDate = x.Order.OrderDate,
                                        OrderStatus = orderStatus,
                                        PhoneNumber = x.Order.PhoneNumber,
                                        TotalPrice = x.Order.TotalPrice,
                                        TotalPriceWithoutDelivery = x.Order.TotalPriceWithoutDelivery,
                                        DelieryPrice = x.Order.DeliveryPrice,
                                        OrderBooks = x.Order.OrderBooks
                                                            .Select(b => new Web.ViewModels.Products.ProductCartDetails
                                                            {
                                                                Publisher = b.Book.Publisher,
                                                                CountOfProduct = b.Count,
                                                                ImageName = b.Book.ImageNameWithExtension != "" ? $"{b.Book.ReleaseDate.Year}/" + b.Book.ImageNameWithExtension : "",
                                                                Discount = b.Discount,
                                                                NameOfProduct = b.Book.Title,
                                                                ProductId = b.Book.BookId,
                                                                Price = b.Price,
                                                                PriceWithDisc = b.PriceWithDisc,
                                                                QuantityPriceWithDisc = b.Count * b.PriceWithDisc
                                                            }).ToArray()
                                    })
                                    .FirstOrDefault();

            if (this.Order == null)
            {
                return Forbid();
            }

            return Page();
        }
    }
}