using AreaBg.Web.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.ViewModels.CombinedViewModels
{
    public class SearchViewModel
    {
        public SearchViewModel()
        {
            this.Products = new List<ProductPartDetailsViewModel>();
        }

        public string Msg { get; set; }

        public List<ProductPartDetailsViewModel> Products { get; set; }
    }
}
