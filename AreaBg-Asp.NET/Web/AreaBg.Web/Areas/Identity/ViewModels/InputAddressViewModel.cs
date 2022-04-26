using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.Areas.Identity.ViewModels
{
    public class InputAddressViewModel
    {
        [Required]
        [Display(Name = "Име")]
        public string Name { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Населено място")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Адрес")]
        public string HomeEkont { get; set; }

        [Required]
        [Display(Name = "Адрес")]
        public string HomeSpeedy { get; set; }

        [Required]
        [Display(Name = "Адрес на Еконт")]
        public string AddressEkont { get; set; }

        [Required]
        [Display(Name = "Адрес на Спиди")]
        public string AddressSpeedy { get; set; }

        [Display(Name = "Забележка")]
        public string Note { get; set; }

        public string ToWhere { get; set; }
    }
}
