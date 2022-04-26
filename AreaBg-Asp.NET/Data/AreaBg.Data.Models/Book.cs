using AreaBg.Data.Models.enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AreaBg.Data.Models
{
    public class Book
    {
        public int BookId { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public int Pages { get; set; }

        public string Format { get; set; }

        public string Publisher { get; set; }

        public ProductCoverEnum Cover { get; set; }

        public decimal Price { get; set; }

        public int Discount { get; set; }

        public int CopyOfDiscount { get; set; }

        public string ImageNameWithExtension { get; set; }

        public int Weight { get; set; }

        public ProductToShowEnum ProductToShow { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string ISBN { get; set; }

        public string BuyFrom { get; set; }

        public string Description { get; set; }

        public int SubCategoryId { get; set; }

        public Subcategory SubCategory { get; set; }

        public int CountOfOrders { get; set; }

        public IsRecommendProductEnum IsRecommendProduct { get; set; }

        public int OrderNumber { get; set; }

        public string AuthorLatin { get; set; }

        public string PublisherLatin { get; set; }

        public ICollection<BookReview> Reviews { get; set; }

        public double Rating { get; set; }
    }
}
