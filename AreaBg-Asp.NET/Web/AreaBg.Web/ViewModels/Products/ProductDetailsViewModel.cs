using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.ViewModels.Products
{
    public class ProductDetailsViewModel
    {
        public int ProductId { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public decimal Price { get; set; }

        public decimal PriceWithDiscount { get; set; }

        public int Discount { get; set; }

        public int Pages { get; set; }

        public string ImageName { get; set; }

        public string Format { get; set; }

        public string Publisher { get; set; }

        public string Cover { get; set; }

        public string ReleaseDate { get; set; }

        public string ISBN { get; set; }

        public string Description { get; set; }

        public int SubcategoryId { get; set; }

        public string SubcategoryTitle { get; set; }

        public string SubcategoryTitleLatin { get; set; }

        public string PublisherLatin { get; set; }

        public string AuthorLatin { get; set; }

        public int ProductToShow { get; set; }

        public DateTime Date { get; set; }
    }
}
