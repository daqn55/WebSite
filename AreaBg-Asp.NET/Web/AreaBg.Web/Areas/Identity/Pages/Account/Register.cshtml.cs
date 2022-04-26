using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AreaBg.Data.Models;
using AreaBg.Web.GlobalsVariables;
using AreaBg.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AreaBg.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<MyIdentityUser> _signInManager;
        private readonly UserManager<MyIdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<MyIdentityUser> userManager,
            SignInManager<MyIdentityUser> signInManager,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Това поле е задължително.")]
            [StringLength(100, ErrorMessage = "Името трябва да е минимум {2} символа.", MinimumLength = 2)]
            [Display(Name = "*Име")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Това поле е задължително.")]
            [EmailAddress(ErrorMessage = "Невалиден имейл.")]
            [Display(Name = "*Имейл")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Това поле е задължително.")]
            [StringLength(100, ErrorMessage = "Паролата трябва да бъде минимум {2} и максимум {1} символа дълга.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "*Парола")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Това поле е задължително.")]
            [DataType(DataType.Password)]
            [Display(Name = "*Потвърди парола")]
            [Compare("Password", ErrorMessage = "Паролите не съвпадат.")]
            public string ConfirmPassword { get; set; }

            [Range(typeof(bool), "true", "true", ErrorMessage = "Не сте се съгласили с общите условия.")]
            [Display(Name = "общите условия")]
            public bool AcceptTerms { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = new MyIdentityUser { UserName = Input.Email, Email = Input.Email, AcceptTerms = Input.AcceptTerms, FirstName = Input.FirstName};
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { userId = user.Id, code = code },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    var mail = new MailService();
                    mail.SendMail(Input.Email, Globals.RegisterNewAccBody, Globals.RegisterNewAccSubject);

                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    var e = error.Code;
                    var msg = "Невалидна регистрация моля опитайте пак";
                    if (e == "DuplicateUserName")
                    {
                        msg = "Вече има регистрация с този имейл";
                    }
                    ModelState.AddModelError(string.Empty, msg);
                }
            }

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
