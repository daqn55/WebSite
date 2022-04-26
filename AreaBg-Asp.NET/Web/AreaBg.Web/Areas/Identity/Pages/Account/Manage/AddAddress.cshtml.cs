using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AreaBg.Data;
using AreaBg.Data.Models;
using AreaBg.Web.Areas.Identity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AreaBg.Web.Areas.Identity.Pages.Account.Manage
{
    public class AddAddressModel : PageModel
    {
        private readonly UserManager<MyIdentityUser> _userManager;
        private readonly MyDbContext _db;

        public AddAddressModel(UserManager<MyIdentityUser> userManager, MyDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        [BindProperty]
        public InputAddressViewModel Input { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/account/login?returnUrl=" + HttpContext.Request.Path);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/account/login?returnUrl=" + HttpContext.Request.Path);
            }

            string addressToDeliver = "";
            var deliveryPlace = Data.Models.enums.DeliveryPlace.Home;

            switch (Input.ToWhere)
            {
                case "OfficeEkont":
                    addressToDeliver = Input.AddressEkont;
                    deliveryPlace = Data.Models.enums.DeliveryPlace.OfficeEkont;
                    break;
                case "OfficeSpeedy":
                    addressToDeliver = Input.AddressSpeedy;
                    deliveryPlace = Data.Models.enums.DeliveryPlace.OfficeSpeedy;
                    break;
                case "toEkont":
                    addressToDeliver = Input.HomeEkont;
                    deliveryPlace = Data.Models.enums.DeliveryPlace.Ekont;
                    break;
                case "toSpeedy":
                    addressToDeliver = Input.HomeSpeedy;
                    deliveryPlace = Data.Models.enums.DeliveryPlace.Speedy;
                    break;
            }

            var address = new Address
            {
                Name = Input.Name,
                PhoneNumber = Input.PhoneNumber,
                City = Input.City,
                Street = addressToDeliver,
                DeliveryPlace = deliveryPlace,
                Note = Input.Note
            };

            var userAddress = new UserAddress
            {
                Address = address,
                User = user
            };

            user.UserAddresses.Add(userAddress);
            await _db.SaveChangesAsync();

            return Redirect("/account/my-addresses");
        }
    }
}