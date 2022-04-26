using AreaBg.Web.ViewModels.CombinedViewModels;
using AreaBg.Web.ViewModels.Products;
using AreaBg.Web.ViewModels.Review;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.Services
{
    public class PaginatedListDetailsPage<T> : List<NewReviewViewModel>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalCount { get; private set; }
        public ProductDetailsViewModel ProductDetails { get; set; }
        public List<ProductPartDetailsViewModel> ProductsPartDetails { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsHaveEnoughSimilarProducts { get; set; }
        public PreviewReviewsViewModel PreviewReviews { get; set; }

        public PaginatedListDetailsPage(List<NewReviewViewModel> reviews, int count, int pageIndex, int pageSize,
            int totalCount, ProductDetailsViewModel productDetails, List<ProductPartDetailsViewModel> productsPartDetails,
            bool isFavorite, bool isHaveEnoughSimilarProducts, PreviewReviewsViewModel previewReviews)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = totalCount;
            this.AddRange(reviews);
            this.ProductDetails = productDetails;
            this.ProductsPartDetails = productsPartDetails;
            this.IsFavorite = isFavorite;
            this.IsHaveEnoughSimilarProducts = isHaveEnoughSimilarProducts;
            this.PreviewReviews = previewReviews;
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public static async Task<PaginatedListDetailsPage<T>> CreateAsync(DetailsAndPartDetailsViewModel source, int pageIndex, int pageSize)
        {
            var productDetails = source.ProductDetails;
            var productsPartDetails = source.ProductsPartDetails;
            var isFavorite = source.IsFavorite;
            var isHaveEnoughSimilarProducts = source.IsHaveEnoughSimilarProducts;
            var previewReviews = source.PreviewReviews;
            var reviews = source.Reviews;

            var totalCount = reviews.Count();

            var count = await reviews.CountAsync();
            var items = await reviews.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedListDetailsPage<T>(items, count, pageIndex, pageSize, totalCount, productDetails, productsPartDetails, isFavorite, isHaveEnoughSimilarProducts, previewReviews);
        }
    }
}
