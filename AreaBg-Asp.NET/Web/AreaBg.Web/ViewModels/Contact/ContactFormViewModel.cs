using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.ViewModels.Contact
{
    public class ContactFormViewModel
    {
        [Required(ErrorMessage ="Полето \"Име\" е задължително.")]
        [StringLength(100, ErrorMessage = "Името трябва да е минимум {2} символа.", MinimumLength = 2)]
        [Display(Name = "*Име")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Полето \"Имейл\" е задължително.")]
        [EmailAddress(ErrorMessage = "Невалиден имейл")]
        [Display(Name = "*Имейл")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Полето \"Телефон\" е задължително.")]
        [Phone(ErrorMessage ="Невалиден телефон.")]
        [Display(Name = "*Телефон")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Полето \"Относно\" е задължително.")]
        [Display(Name = "*Относно")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Полето \"Съобщение\" е задължително.")]
        [Display(Name = "*Съобщение")]
        public string Message { get; set; }
    }
}
