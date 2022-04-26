using AreaBg.Data;
using AreaBg.Data.Models;
using AreaBg.Web.Areas.Admin.Services;
using AreaBg.Web.Areas.Admin.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected IProductAdminService ProductService { get; }

        public BaseController(IProductAdminService productService)
        {
            this.ProductService = productService;
        }
    }
}
