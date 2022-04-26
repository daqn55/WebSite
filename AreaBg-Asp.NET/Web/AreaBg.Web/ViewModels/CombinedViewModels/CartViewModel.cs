using AreaBg.Web.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.ViewModels.CombinedViewModels
{
    public class CartViewModel
    {
        public CartViewModel()
        {
            this.ProductCartDetails = new List<ProductCartDetails>();
        }

        public decimal TotalPrice { get; set; }

        public List<ProductCartDetails> ProductCartDetails { get; set; }
    }
}
