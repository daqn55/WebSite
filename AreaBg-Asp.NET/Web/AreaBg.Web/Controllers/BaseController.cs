using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AreaBg.Web.Areas.Admin.Services.Interfaces;
using AreaBg.Web.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace AreaBg.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IProductAdminService ProductAdminService { get; }
        protected IProductService ProductService { get; }

        public BaseController(IProductAdminService productAdminService, IProductService productService)
        {
            this.ProductAdminService = productAdminService;
            this.ProductService = productService;
        }

        public BaseController(IProductAdminService productAdminService)
        {
            ProductAdminService = productAdminService;
        }
    }
}