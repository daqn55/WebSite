using AreaBg.Web.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.ViewModels.CombinedViewModels
{
    public class FirstPageViewModel
    {
        public FirstPageViewModel()
        {
            this.ProductsFromTeam = new List<ProductPartDetailsViewModel>();
            this.UpcomingProducts = new List<ProductPartDetailsViewModel>();
        }

        public List<ProductPartDetailsViewModel> UpcomingProducts { get; set; }

        public List<ProductPartDetailsViewModel> ProductsFromTeam { get; set; }

        public IQueryable<ProductPartDetailsViewModel> LastProducts { get; set; }

        public int CountLastProducts { get; set; }
        public int CountProductsFromTeam { get; set; }

    }
}
