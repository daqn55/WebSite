using AreaBg.Data.Models;
using AreaBg.Web.Areas.Admin.Services.Interfaces;
using AreaBg.Web.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SimpleMvcSitemap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.Controllers
{
    public class SitemapController : Controller
    {
        private readonly UserManager<MyIdentityUser> UserManager;
        private readonly IProductService ProductService;
        public IConfiguration Configuration { get; }
        public SitemapController(UserManager<MyIdentityUser> userManager, IProductService productService, IConfiguration configuration)
        {
            this.UserManager = userManager;
            this.ProductService = productService;
            this.Configuration = configuration;
        }

        [HttpGet("/sitemap.xml")]
        public IActionResult Site()
        {
            var productCount = int.Parse(this.Configuration["sitemap"]);
            var userId = this.UserManager.GetUserId(this.User);
            var result = this.ProductService.ProductsForSitemap(userId, productCount).ToList();

            return View(result);
        }
    }
}
