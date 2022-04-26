using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.ViewModels.Products
{
    public class ProductPartDetailsViewModel
    {
        public int ProductId { get; set; }

        public string Title { get; set; }

        public string UrlTitle { get; set; }

        public string Author { get; set; }

        public string Publisher { get; set; }

        public bool IsHaveImage { get; set; }

        public string ImageNameWithExtension { get; set; }

        public decimal Price { get; set; }

        public int Discount { get; set; }

        public string ProductToShow { get; set; }

        public int SubcategoryId { get; set; }

        public string SubcategoryTitle { get; set; }

        public string SubcategoryTitleLatin { get; set; }

        public int CountOfOrders { get; set; }

        public bool IsFavorite { get; set; } = false;

        public string AuthorLatin { get; set; }

        public string PublisherLatin { get; set; }

        public int rating { get; set; }

        public DateTime Date { get; set; }
    }
}
