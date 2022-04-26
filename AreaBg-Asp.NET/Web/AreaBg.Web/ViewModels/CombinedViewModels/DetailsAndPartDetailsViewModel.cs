using AreaBg.Web.ViewModels.Products;
using AreaBg.Web.ViewModels.Review;
using System.Collections.Generic;
using System.Linq;

namespace AreaBg.Web.ViewModels.CombinedViewModels
{
    public class DetailsAndPartDetailsViewModel
    {
        public DetailsAndPartDetailsViewModel()
        {
            this.ProductsPartDetails = new List<ProductPartDetailsViewModel>();
        }

        public ProductDetailsViewModel ProductDetails { get; set; }

        public List<ProductPartDetailsViewModel> ProductsPartDetails { get; set; }

        public bool IsHaveEnoughSimilarProducts { get; set; }

        public bool IsFavorite { get; set; } = false;

        public IQueryable<NewReviewViewModel> Reviews { get; set; }

        public PreviewReviewsViewModel PreviewReviews { get; set; }
    }
}
