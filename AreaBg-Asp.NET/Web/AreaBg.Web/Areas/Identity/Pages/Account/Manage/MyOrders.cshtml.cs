using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AreaBg.Data;
using AreaBg.Data.Models;
using AreaBg.Web.Areas.Identity.ViewModels;
using AreaBg.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AreaBg.Web.Areas.Identity.Pages.Account.Manage
{
    public class MyOrdersModel : PageModel
    {
        private readonly MyDbContext _db;
        private readonly UserManager<MyIdentityUser> _userManager;

        public MyOrdersModel(MyDbContext db, UserManager<MyIdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public PaginatedList<MyOrdersViewModel> MyOrders { get; set; }
        public string CurrentSort { get; set; }

        public async Task<IActionResult> OnGetAsync(string sortOrder, int? pageIndex)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Redirect("/account/login?returnUrl=" + HttpContext.Request.Path);
            }
            CurrentSort = sortOrder;

            var res = _db.UserOrders.Where(x => x.UserId == user.Id)
                                     .Include(x => x.Order)
                                     .ThenInclude(x => x.OrderBooks)
                                     .ThenInclude(x => x.Book)
                                     .AsNoTracking()
                                     .AsEnumerable()
                                     .ToList();

            IQueryable<MyOrdersViewModel> myOrders = res.Select(x => new MyOrdersViewModel
            {
                Id = x.OrderId,
                IdClient = x.OrderId + "SA" + x.Order.OrderDate.ToString("MMdd"),
                Address = x.Order.City + ", " + x.Order.Address,
                Date = x.Order.OrderDate.ToString("dd-MM-yyyy") + " г.",
                TotalPrice = x.Order.TotalPrice.ToString("f2"),
                OrderStatus = GlobalsVariables.Globals.OrderStatus[(int)x.Order.OrderStatus]
            })
                                     .OrderByDescending(x => x.Id)
                                     .AsQueryable();

            var pageSize = 10;
            this.MyOrders = PaginatedList<MyOrdersViewModel>.Create(
                myOrders.AsNoTracking(), pageIndex ?? 1, pageSize, "", 0);

            return Page();
        }
    }
}