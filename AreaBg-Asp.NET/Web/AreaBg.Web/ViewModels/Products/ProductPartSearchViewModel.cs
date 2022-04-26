using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.ViewModels.Products
{
    public class ProductPartSearchViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Publisher { get; set; }

        public string ImageEx { get; set; }

        public decimal Price { get; set; }

        public int Discount { get; set; }

        public string TitleLatin { get; set; }

        public string Year { get; set; }
    }
}
