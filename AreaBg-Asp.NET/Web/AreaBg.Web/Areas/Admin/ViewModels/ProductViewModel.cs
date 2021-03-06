using AreaBg.Data.Models.enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.Areas.Admin.ViewModels
{
    public class ProductViewModel
    {
        public int CategorySelectCreateProduct { get; set; }

        public string ProductName { get; set; }

        public string ProductAuthor { get; set; }

        public int ProductPages { get; set; }

        public string ProductFormat { get; set; }

        public string ProductPublisher { get; set; }

        public ProductCoverEnum ProductCover { get; set; }

        public decimal ProductPrice { get; set; }

        public int ProductDiscount { get; set; }

        public IFormFile Image { get; set; }

        public int ProductWeight { get; set; }

        public ProductToShowEnum ProductToShow { get; set; }

        public DateTime ProductDate { get; set; }

        public string ProductISBN { get; set; }

        public string ProductBuyForm { get; set; }

        public string ProductDescription { get; set; }
    }
}
