using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.ViewModels.Review
{
    public class PreviewReviewsViewModel
    {
        public int OneStars { get; set; }

        public int TwoStars { get; set; }

        public int ThreeStars { get; set; }

        public int FourStars { get; set; }

        public int FiveStars { get; set; }

        public string Rating { get; set; }

        public int OnePerc { get; set; }

        public int TwoPerc { get; set; }

        public int ThreePerc { get; set; }

        public int FourPerc { get; set; }

        public int FivePerc { get; set; }

        public int TotalReviews { get; set; }

        public int StarsRating { get; set; }
    }
}
