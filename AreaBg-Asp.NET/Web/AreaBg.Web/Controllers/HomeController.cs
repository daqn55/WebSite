using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AreaBg.Services.DataServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using AreaBg.Web.Areas.Admin.Services.Interfaces;
using AreaBg.Web.ViewModels;
using AreaBg.Web.Interfaces.Services;
using AreaBg.Web.ViewModels.Products;
using AreaBg.Web.ViewModels.CombinedViewModels;
using AreaBg.Web.ViewModels.Contact;
using AreaBg.Data.Models;
using Microsoft.AspNetCore.Identity;
using AreaBg.Web.Services;
using System.Linq;
using AreaBg.Data;
using Microsoft.EntityFrameworkCore;
using AreaBg.Web.ViewModels.Order;
using AreaBg.Web.GlobalsVariables;

namespace AreaBg.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IRoleSevice roleService;
        private readonly UserManager<MyIdentityUser> userManager;
        protected MyDbContext Db { get; }

        public HomeController(MyDbContext db, IProductAdminService productAdminService, IRoleSevice roleService, IProductService productService, UserManager<MyIdentityUser> userManager) : base(productAdminService, productService)
        {
            this.roleService = roleService;
            this.userManager = userManager;
            this.Db = db;
        }

        
        public async Task<IActionResult> Index(string sortOrder, int? p)
        {
            if (p < 1)
            {
                p = 1;
            }

            ViewData["CurrentSort"] = sortOrder;

            //pageNumber = 1;

            var userId = string.Empty;
            if (this.User.Identity.IsAuthenticated)
            {
                userId = this.userManager.GetUserId(this.User);
            }

            var lastProducts = this.ProductService.ShowLastProducts(userId);
            var productsFromTeam = this.ProductService.ShowProductsFromTeam(userId);
            var upcomingProducts = this.ProductService.UpcomingProducts(userId);

            var firstPageViewModel = new FirstPageViewModel
            {
                UpcomingProducts = upcomingProducts,
                LastProducts = lastProducts,
                ProductsFromTeam = productsFromTeam,
                CountLastProducts = lastProducts.Count(),
                CountProductsFromTeam = productsFromTeam.Count
            };

            int pageSize = 20;
            return View(await PaginatedListIndexPage<FirstPageViewModel>.CreateAsync(firstPageViewModel, p ?? 1, pageSize));
        }

        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Privacy()
        {
            //await this.roleService.CreateRole("Admin");
            //await this.roleService.AssingRoleToUser(this.User, "Admin");

            //var jsonModel = await this.Db.Books.Select(x => new JsonSortImagesByYears
            //{
            //    Id = x.BookId,
            //    Year = x.ReleaseDate.Year.ToString()
            //}).ToListAsync();

            //var json = JsonSerializer.Serialize(jsonModel);

            //System.IO.File.WriteAllText(@"G:\SortBgbookPictures\books.json", json, Encoding.UTF8);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("contacts")]
        public IActionResult Contacts()
        {
            //await this.roleService.CreateRole("Admin");
            //await this.roleService.AssingRoleToUser(this.User, "Admin");
            return View();
        }

        [HttpPost("contacts")]
        public async Task<IActionResult> Contacts(ContactFormViewModel model)
        {
            var userId = string.Empty;
            MyIdentityUser user = null;
            var isLogged = this.User.Identity.IsAuthenticated;
            
            if (isLogged)
            {
                user = await this.userManager.GetUserAsync(this.User);
                userId = user.Id;
            }

            var message = this.ProductService.SendMessageToServer(model, userId, user, isLogged);

            TempData["Success"] = "true";
            TempData["Message"] = "Успешно ни изпратихте запитване. Очаквайте в най-скоро време отговор.";

            return this.Redirect("contacts");
        }

        [HttpGet("terms")]
        public IActionResult Terms()
        {
            var terms = this.Db.HelpAndTerms.FirstOrDefault();
            if (terms == null)
            {
                return View();
            }

            var t = new TermsViewModel { data = terms.Terms };

            return View(t);
        }

        [HttpGet("confidentiality")]
        public IActionResult Confidentiality()
        {
            var confid = this.Db.HelpAndTerms.FirstOrDefault();
            if (confid == null)
            {
                return View();
            }

            var c = new TermsViewModel { data = confid.Confidentiality };

            return View(c);
        }

        [HttpGet("about-us")]
        public IActionResult AboutUs()
        {
            var aboutUs = this.Db.HelpAndTerms.FirstOrDefault();
            if (aboutUs == null)
            {
                return View();
            }

            var a = new TermsViewModel { data = aboutUs.AboutUs };


            return View(a);
        }

        [HttpGet("delivery")]
        public IActionResult Delivery()
        {
            var delivery = this.Db.HelpAndTerms.FirstOrDefault();
            if (delivery == null)
            {
                return View();
            }

            var d = new TermsViewModel { data = delivery.DeliveryTerms };


            return View(d);
        }

        [HttpGet("status")]
        public IActionResult OrderStatus()
        {
            return View();
        }

        [HttpPost("status")]
        public IActionResult OrderStatus(string orderId)
        {
            var order = this.Db.Orders
                               .Include(x => x.OrderBooks)
                               .ThenInclude(x => x.Book)
                               .Where(x => x.OrderNumber == orderId)
                               .FirstOrDefault();

            if (orderId == null || order == null)
            {
                TempData["Success"] = "false";
                TempData["Message"] = "Невалиден номер на поръчката, моля опитайте пак.";

                return Redirect("OrderStatus");
            }

            var model = new OrderStatusViewModel
            {
                OrderId = order.OrderNumber,
                TotalPrice = order.TotalPrice.ToString("f2"),
                TotalPriceWithoutDelivery = order.TotalPriceWithoutDelivery.ToString("f2"),
                DeliveryPrice = order.DeliveryPrice.ToString(),
                Status = Globals.OrderStatus[(int)order.OrderStatus],
                Books = order.OrderBooks.Select(x => new ProductStatusViewModel 
                    {
                        Id = x.BookId,
                        Title = x.Book.Title,
                        Author = x.Book.Author,
                        Count = x.Count,
                        Dicount = x.Discount,
                        Price = x.Price,
                        PriceWithDisc = x.PriceWithDisc,
                        QuantityPriceWithDisc = x.Count * x.PriceWithDisc,
                        Publisher = x.Book.Publisher,
                        ImageName = x.Book.ReleaseDate.Year + "/" + x.Book.ImageNameWithExtension
                    }).ToList()
            };

            return View(model);
        }
    }
}
