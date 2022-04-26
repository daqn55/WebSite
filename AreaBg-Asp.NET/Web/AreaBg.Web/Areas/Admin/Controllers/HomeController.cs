using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AreaBg.Data;
using AreaBg.Data.Models;
using AreaBg.Services.DataServices.Interfaces;
using AreaBg.Web.Areas.Admin.Services;
using AreaBg.Web.Areas.Admin.Services.Interfaces;
using AreaBg.Web.Areas.Admin.ViewModels;
using AreaBg.Web.Areas.Admin.ViewModels.Orders;
using AreaBg.Web.Areas.Admin.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AreaBg.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : BaseController
    {
        protected MyDbContext Db { get; }

        public HomeController(IProductAdminService productService, MyDbContext db) : base(productService)
        {
            this.Db = db;
        }

        public IActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated && this.User.IsInRole("Admin"))
            {
                var allCat = ProductService.GetAllCategories();

                return this.View(allCat);
            }

            return Redirect("/Home");
        }

        public IActionResult CategoryList()
        {
            if (this.User.Identity.IsAuthenticated && this.User.IsInRole("Admin"))
            {
                var allCat = ProductService.GetAllCategories();

                return this.View(allCat);
            }

            return Redirect("/Home");
        }

        public IActionResult AddProduct()
        {
            if (this.User.Identity.IsAuthenticated && this.User.IsInRole("Admin"))
            {
                var allCat = ProductService.GetAllCategories();

                return this.View(allCat);
            }

            return Redirect("/Home");
        }

        public IActionResult SearchForProduct()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult MakeCategory(string categoryName)
        {
            if (this.User.Identity.IsAuthenticated && this.User.IsInRole("Admin"))
            {
                var msg = ProductService.MakeCategory(categoryName);

                TempData["Success"] = "true";
                TempData["Message"] = msg;
            }

            return Redirect("/Admin/Home");
        }

        [HttpPost]
        public IActionResult MakeSubCategory(string subCategoryName, string categoryNameWithId)
        {
            if (this.User.Identity.IsAuthenticated && this.User.IsInRole("Admin"))
            {
                var msg = ProductService.MakeSubCategory(subCategoryName, categoryNameWithId);

                TempData["Success"] = "true";
                TempData["Message"] = msg;
            }

            return Redirect("/Admin/Home");
        }

        [HttpPost]
        public IActionResult AddProduct(ProductViewModel productViewModel)
        {
            if (this.User.Identity.IsAuthenticated && this.User.IsInRole("Admin"))
            {
                var msg = ProductService.AddProduct(productViewModel);

                TempData["Success"] = "true";
                TempData["Message"] = msg;
            }

            return Redirect("/Admin/Home");
        }

        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            var searchedBook = ProductService.GetProductDetailsToEdit(id);
            var date = searchedBook.Book.ReleaseDate.Date;
            return this.View(searchedBook);
        }

        [HttpPost]
        public IActionResult EditProduct(EditProductViewModel productViewModel)
        {
            var productMsg = ProductService.EditProduct(productViewModel);

            TempData["Success"] = "true";
            TempData["Message"] = productMsg;

            return Redirect("/Admin/Home/SearchForProduct");
        }

        [HttpGet]
        public IActionResult SearchProduct(string productToSearch, string searchOption)
        {
            var products = ProductService.SearchProduct(productToSearch, searchOption);

            return this.View("SearchForProduct", products);
        }

        [HttpPost]
        public IActionResult UpdateCategoryOrder(IFormCollection orderNumbers)
        {
            var msg = this.ProductService.UpdateCategoryOrder(orderNumbers);

            return this.Redirect("/Admin/Home/CategoryList");
        }

        public IActionResult Roles(string clientToSearch, string searchOption)
        {
            if (clientToSearch != null)
            {
                var terms = clientToSearch.ToLower().Split(" ");

                if (searchOption == "byEmail")
                {
                    var user = this.Db.Users.AsEnumerable().Where(x => x.Email != null).Where(x => terms.Any(t => x.Email.ToLower().Contains(t))).ToList();


                    var model = user.OrderByDescending(x => x.Id)
                                    .Select(x => new UserViewModel
                                    {
                                        Id = x.Id,
                                        Email = x.Email,
                                        Name = x.FirstName
                                    })
                                    .ToList();

                    return View(model);
                }
            }

            return View();
        }

        public IActionResult CreateAdmin(string id)
        {
            var roleId = this.Db.Roles.Where(x => x.Name == "Admin").Select(x => x.Id).First();
            var userRole = new IdentityUserRole<string> { UserId = id, RoleId = roleId };
            var user = this.Db.UserRoles.Add(userRole);
            this.Db.SaveChanges();
            
            return Redirect("Roles");
        }
    }
}