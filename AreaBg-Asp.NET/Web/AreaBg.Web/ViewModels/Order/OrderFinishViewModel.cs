using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.ViewModels.Order
{
    public class OrderFinishViewModel
    {
        [BindProperty, Required(ErrorMessage = "*Моля изберете начина на доставка")]
        public string ChooseAddress { get; set; }

        [BindProperty, Required(ErrorMessage = "*Моля изберете начина на доставка")]
        public string DeliveryOption { get; set; }

        [BindProperty, Required(ErrorMessage = "*Моля изберете адрес за доставка")]
        public string AddressId { get; set; }

        public string OrderNote { get; set; }

        [BindProperty, Required(ErrorMessage = "*Моля въведете име.")]
        public string Name { get; set; }

        [BindProperty, Required(ErrorMessage = "*Моля въведете телефон за връзка.")]
        public string PhoneNumber { get; set; }

        [BindProperty, Required(ErrorMessage = "*Моля въведете град.")]
        public string City { get; set; }

        [BindProperty, Required(ErrorMessage = "*Моля въведете адрес за доставка")]
        public string DeliveryAddress { get; set; }

        [BindProperty, EmailAddress, Required(ErrorMessage = "*Моля въведете валиден имейл адрес.")]
        public string Email { get; set; }

        public string Price { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "*Не сте се съгласили с общите условия.")]
        [Display(Name = "общите условия")]
        public bool AcceptTerms { get; set; }

        public string Paying { get; set; }

        public double DeliveryPrice { get; set; }
    }
}
