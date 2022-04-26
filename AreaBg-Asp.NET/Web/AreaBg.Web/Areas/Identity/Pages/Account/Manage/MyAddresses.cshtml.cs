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

namespace AreaBg.Web.Areas.Identity.Pages.Account.Manage
{
    public class MyAddressesModel : PageModel
    {
        private readonly MyDbContext _db;
        private readonly UserManager<MyIdentityUser> _userManager;

        public MyAddressesModel(MyDbContext db, UserManager<MyIdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public List<MyAddressViewModel> MyAddresses { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Redirect("/account/login?returnUrl=" + HttpContext.Request.Path);
            }

            MyAddresses = _db.UserAddresses.Where(x => x.UserId == user.Id)
                                           .Include(x => x.Address)
                                           .AsNoTracking()
                                           .Select(x => new MyAddressViewModel
                                           {
                                               Id = x.Address.Id,
                                               Address = x.Address.Street,
                                               City = x.Address.City,
                                               DeliveryPlace = x.Address.DeliveryPlace,
                                               Name = x.Address.Name,
                                               Note = x.Address.Note,
                                               Phone = x.Address.PhoneNumber
                                           }).ToList();

            return Page();
        }

        public IActionResult OnGetDeleteAddress(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var address = _db.Addresses.Find(id);
                _db.Remove(address);
                _db.SaveChanges();
            }

            return RedirectToPage();
        }
    }
}