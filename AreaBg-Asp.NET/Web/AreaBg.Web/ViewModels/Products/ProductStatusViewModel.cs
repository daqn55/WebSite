using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.ViewModels.Products
{
    public class ProductStatusViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public decimal Price { get; set; }

        public decimal PriceWithDisc { get; set; }

        public string Publisher { get; set; }

        public decimal QuantityPriceWithDisc { get; set; }

        public string ImageName { get; set; }

        public int Dicount { get; set; }

        public int Count { get; set; }
    }
}
