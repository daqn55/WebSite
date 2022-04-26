using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AreaBg.Data;
using AreaBg.Data.Models;
using AreaBg.Web.Services;
using AreaBg.Web.ViewModels.CombinedViewModels;
using AreaBg.Web.ViewModels.Products;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AreaBg.Web.Controllers
{
    //[Route("/[controller]/[action]")]
    public class SearchController : Controller
    {
        private readonly MyDbContext Db;
        private readonly UserManager<MyIdentityUser> userManager;

        public SearchController(MyDbContext db, UserManager<MyIdentityUser> userManager)
        {
            Db = db;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("search")]
        public IActionResult Find(string keyword, string sortOrder, string f, int? p)
        {
            if (p < 1)
            {
                p = 1;
            }

            ViewData["CurrentSort"] = sortOrder;

            if (keyword != null)
            {
                p = 1;
            }
            else
            {
                keyword = f;
            }

            ViewData["CurrentFilter"] = keyword;

            var msg = string.Empty;

            if (string.IsNullOrEmpty(keyword) || string.IsNullOrWhiteSpace(keyword))
            {
                msg = "";
                return View("~/Views/Search/Result.cshtml", null);
            }

            var userId = string.Empty;
            if (this.User.Identity.IsAuthenticated)
            {
                userId = this.userManager.GetUserId(this.User);
            }

            IQueryable<ProductPartDetailsViewModel> results = null;
            var userFavorites = this.Db.UserFavoriteProducts.Where(x => x.UserId == userId).Select(x => x.BookId).ToList();

            var terms = keyword.ToLower().Split(" ").ToArray();
            var k = keyword.ToLower().Trim();
            var res = this.Db.Books
                                .Include(x => x.SubCategory)
                                .AsNoTracking()
                                .AsEnumerable()
                                .Where(x => (x.Title != null ? x.Title.ToLower().Contains(k) : "".Contains(k))
                                                               || (x.Author != null ? x.Author.ToLower().Contains(k) : "".Contains(k))
                                                               || (x.Publisher != null ? x.Publisher.ToLower().Contains(k) : "".Contains(k)))
                                .OrderByDescending(x => x.BookId)
                                .Take(2000)
                                .ToList();
                                //.Where(x => terms.Any(t => t.All(k => x.Title != null ? x.Title.ToLower().Contains(k) : "".Contains(k)
                                //                               || x.Author != null ? x.Author.ToLower().Contains(k) : "".Contains(k)
                                //                               || x.Publisher != null ? x.Publisher.ToLower().Contains(k) : "".Contains(k)
                                //                               && x.ProductToShow == Data.Models.enums.ProductToShowEnum.Yes)))
                                //.OrderByDescending(x => x.BookId)
                                //.Take(2000)
                                //.ToList();
                                //.Where(x => terms.All(t => x.Title != null ? x.Title.ToLower().Contains(t) : "".Contains(t)
                                //                               || x.Author != null ? x.Author.ToLower().Contains(t) : "".Contains(t)
                                //                               || x.Publisher != null ? x.Publisher.ToLower().Contains(t) : "".Contains(t))
                                //                               && x.ProductToShow == Data.Models.enums.ProductToShowEnum.Yes)
                                //.OrderByDescending(x => x.BookId)
                                //.Take(2000)
                                //.ToList();

            results = res
                        .Select(x => new ProductPartDetailsViewModel
                        {
                            Author = x.Author,
                            Title = x.Title,
                            CountOfOrders = x.CountOfOrders,
                            Discount = x.Discount,
                            ImageNameWithExtension = x.ImageNameWithExtension != "" ? $"{x.ReleaseDate.Year}/" + x.ImageNameWithExtension : "",
                            IsHaveImage = string.IsNullOrEmpty(x.ImageNameWithExtension),
                            Price = x.Price,
                            ProductId = x.BookId,
                            ProductToShow = x.ProductToShow.ToString(),
                            Publisher = x.Publisher,
                            SubcategoryId = x.SubCategory.SubcategoryId,
                            SubcategoryTitle = x.SubCategory.Title,
                            IsFavorite = userFavorites.Contains(x.BookId),
                            UrlTitle = ConvertCharsService.CyrillicToLatinChars(x.Title),
                            AuthorLatin = x.AuthorLatin,
                            PublisherLatin = x.PublisherLatin,
                            rating = (int)Math.Floor(x.Rating)
                        })
                        .AsQueryable();

            msg = keyword;

            var countOfProducts = results.Count();
            int pageSize = 20;
            return View("~/Views/Search/Result.cshtml", PaginatedList<ProductPartDetailsViewModel>.Create(results.AsNoTracking(), p ?? 1, pageSize, msg, countOfProducts));
        }

        [HttpGet("find/{searchBy}/{id}/{keyword}")]
        public IActionResult By(int id, string keyword, string searchBy, string sortOrder, string currentFilter, int? p)
        {
            if (p < 1)
            {
                p = 1;
            }

            var msg = string.Empty;

            if (string.IsNullOrEmpty(keyword) || string.IsNullOrWhiteSpace(keyword))
            {
                msg = "";
                return View("~/Views/Search/ResultSub.cshtml", null);
            }

            var userId = string.Empty;
            if (this.User.Identity.IsAuthenticated)
            {
                userId = this.userManager.GetUserId(this.User);
            }

            IQueryable<ProductPartDetailsViewModel> results = null;
            var userFavorites = this.Db.UserFavoriteProducts.Where(x => x.UserId == userId).Select(x => x.BookId).ToList();

            var res = SearchResults(id, keyword, searchBy, userFavorites);
            results = res.Item1;
            msg = res.Item2;

            var countOfProducts = results.Count();
            int pageSize = 20;
            return View("~/Views/Search/ResultSub.cshtml", PaginatedList<ProductPartDetailsViewModel>.Create(results.AsNoTracking(), p ?? 1, pageSize, msg, countOfProducts));

        }

        [HttpGet("search/{searchBy}/{keyword}")]
        public IActionResult For(string keyword, string searchBy, string sortOrder, string currentFilter, int? p)
        {
            if (p < 1)
            {
                p = 1;
            }

            var msg = string.Empty;

            if (string.IsNullOrEmpty(keyword) || string.IsNullOrWhiteSpace(keyword))
            {
                msg = "";
                return View("~/Views/Search/ResultPub.cshtml", null);
            }

            var userId = string.Empty;
            if (this.User.Identity.IsAuthenticated)
            {
                userId = this.userManager.GetUserId(this.User);
            }

            IQueryable<ProductPartDetailsViewModel> results = null;
            var userFavorites = this.Db.UserFavoriteProducts.Where(x => x.UserId == userId).Select(x => x.BookId).ToList();

            var res = SearchResults(0, keyword, searchBy, userFavorites);
            results = res.Item1;
            msg = res.Item2;

            if (results != null)
            {
                var countOfProducts = results.Count();
                int pageSize = 20;
                return View("~/Views/Search/ResultPub.cshtml", PaginatedList<ProductPartDetailsViewModel>.Create(results.AsNoTracking(), p ?? 1, pageSize, msg, countOfProducts));
            }

            return View("~/Views/Home/Index.cshtml");
        }

        private Tuple<IQueryable<ProductPartDetailsViewModel>, string> SearchResults(int id, string keyword, string searchBy, List<int> userFavorite)
        {
            IQueryable<ProductPartDetailsViewModel> model = null;
            string msg = string.Empty;

            if (keyword != null)
            {
                var books = this.Db.Books;
                switch (searchBy)
                {
                    case "publisher":
                        model = books.Where(x => x.PublisherLatin == keyword)
                                        .Include(x => x.SubCategory)
                                        .AsNoTracking()
                                        .Select(x => new ProductPartDetailsViewModel
                                        {
                                            Author = x.Author,
                                            Title = x.Title,
                                            CountOfOrders = x.CountOfOrders,
                                            Discount = x.Discount,
                                            ImageNameWithExtension = x.ImageNameWithExtension != "" ? $"{x.ReleaseDate.Year}/" + x.ImageNameWithExtension : "",
                                            IsHaveImage = string.IsNullOrEmpty(x.ImageNameWithExtension),
                                            Price = x.Price,
                                            ProductId = x.BookId,
                                            ProductToShow = x.ProductToShow.ToString(),
                                            Publisher = x.Publisher,
                                            SubcategoryId = x.SubCategory.SubcategoryId,
                                            SubcategoryTitle = x.SubCategory.Title,
                                            IsFavorite = userFavorite.Contains(x.BookId),
                                            UrlTitle = ConvertCharsService.CyrillicToLatinChars(x.Title),
                                            AuthorLatin = x.AuthorLatin,
                                            PublisherLatin = x.PublisherLatin,
                                            rating = (int)Math.Floor(x.Rating)
                                        })
                                        .OrderByDescending(x => x.ProductId);
                        var publisher = model.FirstOrDefault();
                        if (publisher == null)
                        {
                            msg = "издателство " + "\"" + keyword + "\"";
                        }
                        else
                        {
                            msg = "издателство " + "\"" + publisher.Publisher + "\"";
                        }
                        break;
                    case "author":
                        model = books.Where(x => x.AuthorLatin == keyword)
                                        .Include(x => x.SubCategory)
                                        .AsNoTracking()
                                        .Select(x => new ProductPartDetailsViewModel
                                        {
                                            Author = x.Author,
                                            Title = x.Title,
                                            CountOfOrders = x.CountOfOrders,
                                            Discount = x.Discount,
                                            ImageNameWithExtension = x.ImageNameWithExtension != "" ? $"{x.ReleaseDate.Year}/" + x.ImageNameWithExtension : "",
                                            IsHaveImage = string.IsNullOrEmpty(x.ImageNameWithExtension),
                                            Price = x.Price,
                                            ProductId = x.BookId,
                                            ProductToShow = x.ProductToShow.ToString(),
                                            Publisher = x.Publisher,
                                            SubcategoryId = x.SubCategory.SubcategoryId,
                                            SubcategoryTitle = x.SubCategory.Title,
                                            IsFavorite = userFavorite.Contains(x.BookId),
                                            UrlTitle = ConvertCharsService.CyrillicToLatinChars(x.Title),
                                            AuthorLatin = x.AuthorLatin,
                                            PublisherLatin = x.PublisherLatin,
                                            rating = (int)Math.Floor(x.Rating)
                                        })
                                        .OrderByDescending(x => x.ProductId);
                        var author = model.FirstOrDefault();
                        if (author == null)
                        {
                            msg = "автор " + "\"" + keyword + "\"";
                        }
                        else
                        {
                            msg = "автор " + "\"" + author.Author + "\"";
                        }
                        break;
                    case "subcat":
                        model = books
                                        .Include(x => x.SubCategory)
                                        .Where(x => x.SubCategory.SubcategoryId == id)
                                        .AsNoTracking()
                                        .Select(x => new ProductPartDetailsViewModel
                                        {
                                            Author = x.Author,
                                            Title = x.Title,
                                            CountOfOrders = x.CountOfOrders,
                                            Discount = x.Discount,
                                            ImageNameWithExtension = x.ImageNameWithExtension != "" ? $"{x.ReleaseDate.Year}/" + x.ImageNameWithExtension : "",
                                            IsHaveImage = string.IsNullOrEmpty(x.ImageNameWithExtension),
                                            Price = x.Price,
                                            ProductId = x.BookId,
                                            ProductToShow = x.ProductToShow.ToString(),
                                            Publisher = x.Publisher,
                                            SubcategoryId = x.SubCategory.SubcategoryId,
                                            SubcategoryTitle = x.SubCategory.Title,
                                            IsFavorite = userFavorite.Contains(x.BookId),
                                            UrlTitle = ConvertCharsService.CyrillicToLatinChars(x.Title),
                                            AuthorLatin = x.AuthorLatin,
                                            PublisherLatin = x.PublisherLatin,
                                            rating = (int)Math.Floor(x.Rating)
                                        })
                                        .OrderByDescending(x => x.ProductId);
                        var subTitle = model.FirstOrDefault();
                        if (subTitle == null)
                        {
                            msg = "подкатегория " + "\"" + keyword + "\"";
                        }
                        else
                        {
                            msg = "подкатегория " + "\"" + subTitle.SubcategoryTitle + "\"";
                        }
                        break;
                }
            }

            return Tuple.Create(model, msg);
        }

        public IActionResult tempSearchData(string keyword)
        {
            var terms = keyword.ToLower().Split(" ");

            var result = this.Db.Books
                                    .Include(x => x.SubCategory)
                                    .AsNoTracking()
                                    .AsEnumerable()
                                    .Where(x => terms.All(t => (x.Title != null ? x.Title.ToLower().Contains(t) : "".Contains(t))
                                                               || (x.Author != null ? x.Author.ToLower().Contains(t) : "".Contains(t))
                                                               || (x.Publisher != null ? x.Publisher.ToLower().Contains(t) : "".Contains(t)))
                                                               && x.ProductToShow == Data.Models.enums.ProductToShowEnum.Yes)
                                    .OrderByDescending(x => x.BookId)
                                    .Select(x => new ProductPartSearchViewModel
                                    {
                                        Author = x.Author,
                                        Title = x.Title,
                                        Discount = x.Discount,
                                        ImageEx = x.ImageNameWithExtension,
                                        Price = x.Price,
                                        Id = x.BookId,
                                        Publisher = x.Publisher,
                                        TitleLatin = ConvertCharsService.CyrillicToLatinChars(x.Title),
                                        Year = x.ReleaseDate.Year.ToString()
                                    })
                                    .Take(6)
                                    .ToList();

            var htmlResult = string.Empty;
            foreach (var b in result)
            {
                var imageEx = b.ImageEx;
                if (String.IsNullOrEmpty(imageEx) || String.IsNullOrWhiteSpace(imageEx))
                {
                    imageEx = "NoImage.png";
                }
                else
                {
                    imageEx = $"{b.Year}/" + b.ImageEx;
                }

                htmlResult += $"<tr class='cart-hv clickable-row' data-href='/product/{b.Id}/{b.TitleLatin}'><th style = 'width: 10%' scope ='row' class='pt-1 pb-1'><img src = '/images/{imageEx}' class='food6'></th><td style = 'width: 60%' class='align-middle'><span class='prpColor'>{b.Title}: {b.Author}</span></td><td style ='width: 30%' class='align-middle'>";

                if (b.Discount > 0)
                {
                    decimal priceWithDisc = (b.Price * (100 - b.Discount)) / 100;
                    htmlResult += $"<small class='font-spCom'><s>{b.Price.ToString("f2")}</s></small><span class='font-spCom' style = 'color:#dc3545'>&nbsp;{priceWithDisc.ToString("f2")} лв.</span></td></tr>";
                }
                else
                {
                    htmlResult += $"<span class='font-spCom'>{b.Price} лв.</span></td></tr>";
                }
            }

            return Content(htmlResult);
        }
    }
}