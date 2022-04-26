using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AreaBg.Data;
using AreaBg.Data.Models;
using AreaBg.Web.Areas.Admin.Services.Interfaces;
using AreaBg.Web.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AreaBg.Web.Controllers
{
    public class FavoriteController : BaseController
    {
        private readonly MyDbContext _db;
        private readonly UserManager<MyIdentityUser> _userManager;

        public FavoriteController(IProductAdminService productAdminService, IProductService productService, MyDbContext db, UserManager<MyIdentityUser> userManager) : base(productAdminService, productService)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult AddToFavorite(int id)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(User);
                var book = _db.Books.Find(id);

                var favoriteProduct = new UserFavoriteProduct
                {
                    Book = book,
                    UserId = userId
                };

                _db.Users.Find(userId).UserFavoriteProducts.Add(favoriteProduct);
                _db.SaveChanges();

                return Redirect(Request.Headers["Referer"].ToString());
            }
            else
            {
                return Redirect("/Identity/Account/Login");
            }
            
        }


        public IActionResult DeleteFavorite(int id)
        {
            var msg = String.Empty;

            if (User.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(User);
                var favProduct = _db.UserFavoriteProducts.Where(x => x.UserId == userId && x.BookId == id).FirstOrDefault();
                if (favProduct != null)
                {
                    _db.UserFavoriteProducts.Remove(favProduct);
                    _db.SaveChanges();
                    msg = "Успешно премахнахте продукта от любими";
                }
                else
                {
                    msg = "Нямате право да премахнете този продукт от любими!";
                }

                //return new JsonResult(msg);
                return Redirect(Request.Headers["Referer"].ToString());
            }
            else
            {
                return Redirect("/Identity/Account/Login");
            }
        }
    }
}