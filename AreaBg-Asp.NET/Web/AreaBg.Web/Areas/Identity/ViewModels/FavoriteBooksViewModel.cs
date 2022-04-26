using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.Areas.Identity.ViewModels
{
    public class FavoriteBooksViewModel
    {
        public int Count { get; set; }

        public int BookId { get; set; }

        public string Title { get; set; }

        public string TitleLatin { get; set; }

        public string Author { get; set; }

        public string Publisher { get; set; }

        public string ImageName { get; set; }

        public int Discount { get; set; }

        public decimal Price { get; set; }
    }
}
