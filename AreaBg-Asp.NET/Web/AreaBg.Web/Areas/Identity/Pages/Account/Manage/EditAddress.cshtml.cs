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

namespace AreaBg.Web.Areas.Identity.Pages.Account.Manage
{
    public class EditAddressModel : PageModel
    {
        private readonly UserManager<MyIdentityUser> _userManager;
        private readonly MyDbContext _db;

        public EditAddressModel(UserManager<MyIdentityUser> userManager, MyDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        [BindProperty]
        public InputAddressViewModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/account/login?returnUrl=" + HttpContext.Request.Path);
            }

            var address = _db.Addresses.Find(id);
            Input = new InputAddressViewModel
            {
                City = address.City,
                Name = address.Name,
                PhoneNumber = address.PhoneNumber,
                Note = address.Note,
            };

            if (address.DeliveryPlace == Data.Models.enums.DeliveryPlace.OfficeSpeedy)
            {
                Input.ToWhere = "toOfficeSpeedy";
                Input.AddressSpeedy = address.Street;
            }
            else if (address.DeliveryPlace == Data.Models.enums.DeliveryPlace.OfficeEkont)
            {
                Input.ToWhere = "toOfficeEkont";
                Input.AddressEkont = address.Street;
            }
            else if (address.DeliveryPlace == Data.Models.enums.DeliveryPlace.Speedy)
            {
                Input.ToWhere = "toSpeedy";
                Input.HomeSpeedy = address.Street;
            }
            else if (address.DeliveryPlace == Data.Models.enums.DeliveryPlace.Ekont)
            {
                Input.ToWhere = "toEkont";
                Input.HomeEkont = address.Street;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
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
                case "toOfficeSpeedy":
                    addressToDeliver = Input.AddressSpeedy;
                    deliveryPlace = Data.Models.enums.DeliveryPlace.OfficeSpeedy;
                    break;
                case "toOfficeEkont":
                    addressToDeliver = Input.AddressEkont;
                    deliveryPlace = Data.Models.enums.DeliveryPlace.OfficeEkont;
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

            var address = _db.Addresses.Find(id);
            address.City = Input.City;
            address.DeliveryPlace = deliveryPlace;
            address.Name = Input.Name;
            address.Note = Input.Note;
            address.PhoneNumber = Input.PhoneNumber;
            address.Street = addressToDeliver;

            _db.Addresses.Update(address);
            _db.SaveChanges();

            return Redirect("/account/my-addresses");
        }
    }
}