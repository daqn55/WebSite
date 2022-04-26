using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AreaBg.Web.Areas.Admin.Services.Interfaces;
using AreaBg.Web.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace AreaBg.Web.Controllers
{
    public class CategoryController : BaseController
    {
        public CategoryController(IProductAdminService productAdminService, IProductService productService) : base(productAdminService, productService)
        {
        }

        public IActionResult Subcategory(int id)
        {

            return View();
        }
    }
}