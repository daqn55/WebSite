using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.ViewModels.Products
{
    public class ProductCartDetails
    {
        public int ProductId { get; set; }

        public string NameOfProduct { get; set; }

        public string TitleLatin { get; set; }

        public string Author { get; set; }

        public string Publisher { get; set; }

        public decimal Price { get; set; }

        public decimal PriceWithDisc { get; set; }

        public decimal QuantityPriceWithDisc { get; set; }

        public int Discount { get; set; }

        public int CountOfProduct { get; set; }

        public string ImageName { get; set; }
    }
}
