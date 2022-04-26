using AreaBg.Web.Areas.Admin.Services;
using AreaBg.Web.Areas.Admin.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.ViewComponents
{
    public class NavigationViewComponent : ViewComponent
    {
        protected IProductAdminService ProductService { get; }

        public NavigationViewComponent(IProductAdminService productService)
        {
            this.ProductService = productService;
        }

        public IViewComponentResult Invoke()
        {
            var categories = this.ProductService.GetAllCategories();

            return this.View(categories);
        }
    }
}
