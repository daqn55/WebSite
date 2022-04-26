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
    public class FavoriteProductsModel : PageModel
    {
        private readonly MyDbContext _db;
        private readonly UserManager<MyIdentityUser> _userManager;

        public FavoriteProductsModel(MyDbContext db, UserManager<MyIdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [BindProperty]
        public List<FavoriteBooksViewModel> FavoriteBooks { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Redirect("/Identity/Account/Login?returnUrl=" + HttpContext.Request.Path);
            }

            FavoriteBooks = _db.UserFavoriteProducts
                               .Include(x => x.Book)
                               .Where(x => x.UserId == user.Id)
                               .AsNoTracking()
                               .Select(x => new FavoriteBooksViewModel
                               {
                                   Count = x.Id,
                                   BookId = x.BookId,
                                   Author = x.Book.Author,
                                   Discount = x.Book.Discount,
                                   ImageName = x.Book.ImageNameWithExtension != "" ? $"{x.Book.ReleaseDate.Year}/" + x.Book.ImageNameWithExtension : "",
                                   Price = x.Book.Price,
                                   Publisher = x.Book.Publisher,
                                   Title = x.Book.Title,
                                   TitleLatin = ConvertCharsService.CyrillicToLatinChars(x.Book.Title)
                               })
                               .ToList();

            return Page();
        }
    }
}