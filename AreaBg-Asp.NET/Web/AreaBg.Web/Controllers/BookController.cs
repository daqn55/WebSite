using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AreaBg.Data;
using AreaBg.Data.Models;
using AreaBg.Web.Areas.Admin.Services.Interfaces;
using AreaBg.Web.Interfaces.Services;
using AreaBg.Web.Services;
using AreaBg.Web.ViewModels.CombinedViewModels;
using AreaBg.Web.ViewModels.Products;
using AreaBg.Web.ViewModels.Review;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AreaBg.Web.Controllers
{
    public class BookController : BaseController
    {
        private readonly UserManager<MyIdentityUser> userManager;

        private readonly MyDbContext Db;

        private readonly ICaptchaValidator CaptchaValidator;

        public BookController(IProductAdminService productAdminService, IProductService productService,
            UserManager<MyIdentityUser> userManager, MyDbContext db,
            ICaptchaValidator captchaValidator) : base(productAdminService, productService)
        {
            this.userManager = userManager;
            this.Db = db;
            this.CaptchaValidator = captchaValidator;
        }

        [HttpGet("Book/BookOrderDetails")]
        public IActionResult BookOrderDetails(int id)
        {
            var productDetails = this.ProductService.GetOrderIndexDetails(id);

            return new JsonResult(productDetails);
        }

        [HttpGet("product/{id}/{title}")]
        public async Task<IActionResult> Details(int id, string title, int? p, int? star)
        {
            if (p < 1)
            {
                p = 1;
            }

            var userId = string.Empty;
            if (this.User.Identity.IsAuthenticated)
            {
                userId = this.userManager.GetUserId(this.User);
            }

            var productDetails = this.ProductService.AllProductDetails(id);
            var similarProducts = this.ProductService.SimilarProducts(id, userId);
            var IsHaveSimilarProducts = true;
            if (similarProducts == null)
            {
                IsHaveSimilarProducts = false;
            }

            var isFavoriteProduct = this.ProductService.isProductFavorite(id, userId);

            var starsAndRating = getStarsAndRating(id);
            var stars = starsAndRating.Item1;
            var reviews = starsAndRating.Item2;
            var rating = starsAndRating.Item3;

            if (star != null)
            {
                reviews = reviews.Where(x => x.stars == star).Select(x => x);
            }

            var one = stars[0];
            var two = stars[1];
            var three = stars[2];
            var four = stars[3];
            var five = stars[4];

            var totalReviews = one + two + three + four + five;

            var preview = new PreviewReviewsViewModel
            {
                OneStars = one,
                TwoStars = two,
                ThreeStars = three,
                FourStars = four,
                FiveStars = five,
                Rating = rating.ToString("f1"),
                TotalReviews = totalReviews,
                OnePerc = one > 0 ? (int)(((double)one / totalReviews) * 100) : 0,
                TwoPerc = two > 0 ? (int)(((double)two / totalReviews) * 100) : 0,
                ThreePerc = three > 0 ? (int)(((double)three / totalReviews) * 100) : 0,
                FourPerc = four > 0 ? (int)(((double)four / totalReviews) * 100) : 0,
                FivePerc = five > 0 ? (int)(((double)five / totalReviews) * 100) : 0,
                StarsRating = (int)Math.Floor(rating)
            };

            var detailsAndPartDetails = new DetailsAndPartDetailsViewModel
            {
                ProductDetails = productDetails,
                ProductsPartDetails = similarProducts,
                IsHaveEnoughSimilarProducts = IsHaveSimilarProducts,
                IsFavorite = isFavoriteProduct,
                Reviews = reviews,
                PreviewReviews = preview
            };

            int pageSize = 6;
            return View("~/Views/Product/Details.cshtml", await PaginatedListDetailsPage<DetailsAndPartDetailsViewModel>.CreateAsync(detailsAndPartDetails, p ?? 1, pageSize));
            //return this.View("~/Views/Product/Details.cshtml", detailsAndPartDetails);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(NewReviewViewModel model)
        {
            if (!await this.CaptchaValidator.IsCaptchaPassedAsync(model.captcha))
            {
                ModelState.AddModelError("captcha", "Captcha validation failed");

                return Redirect(Request.Headers["Referer"].ToString());
            }

            var date = DateTime.Today;

            var newReview = new BookReview
            {
                BookId = model.bookId,
                Body = model.reviewBody,
                date = date,
                Name = model.reviewName,
                Subject = model.reviewSubject,
                Stars = model.stars
            };

            this.Db.BookReviews.Add(newReview);
            this.Db.SaveChanges();

            var book = this.Db.Books.Find(model.bookId);

            var starsAndRating = getStarsAndRating(model.bookId);
            var rating = starsAndRating.Item3;

            book.Rating = rating;

            this.Db.Books.Update(book);
            this.Db.SaveChanges();

            return Redirect(Request.Headers["Referer"].ToString());
        }

        private Tuple<List<int>, IQueryable<NewReviewViewModel>, double>  getStarsAndRating(int bookId)
        {
            var reviews = this.Db.BookReviews
                              .Where(x => x.BookId == bookId)
                              .OrderByDescending(x => x.Id)
                              .AsNoTracking()
                              .Select(x => new NewReviewViewModel
                              {
                                  bookId = bookId,
                                  stars = x.Stars,
                                  reviewBody = x.Body,
                                  reviewName = x.Name,
                                  reviewSubject = x.Subject,
                                  Date = x.date.ToString("dd-MM-yyyy")
                              });

            var one = reviews.Where(x => x.stars == 1).Count();
            var two = reviews.Where(x => x.stars == 2).Count();
            var three = reviews.Where(x => x.stars == 3).Count();
            var four = reviews.Where(x => x.stars == 4).Count();
            var five = reviews.Where(x => x.stars == 5).Count();

            double rating = (double)(5 * five + 4 * four + 3 * three + 2 * two + 1 * one) / (double)(five + four + three + two + one);

            var starts = new List<int> { one, two, three, four, five };

            return Tuple.Create(starts, reviews, rating);
        }
    }
}