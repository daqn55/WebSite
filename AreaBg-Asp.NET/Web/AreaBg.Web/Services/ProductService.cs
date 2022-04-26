using AreaBg.Data;
using AreaBg.Data.Models;
using AreaBg.Data.Models.enums;
using AreaBg.Web.Interfaces.Services;
using AreaBg.Web.ViewModels.Contact;
using AreaBg.Web.ViewModels.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AreaBg.Web.Services
{
    public class ProductService : IProductService
    {
        protected MyDbContext Db { get; }
        private readonly IHostEnvironment webHostEnvironment;

        public ProductService(MyDbContext db, IHostEnvironment hostEnvironment)
        {
            this.Db = db;
            this.webHostEnvironment = hostEnvironment;
        }

        public List<ProductPartDetailsViewModel> UpcomingProducts(string userId)
        {
            var userFavorites = new List<int>();

            if (!string.IsNullOrEmpty(userId) || !string.IsNullOrWhiteSpace(userId))
            {
                userFavorites = this.Db.UserFavoriteProducts.Where(x => x.UserId == userId).AsNoTracking().Select(x => x.BookId).ToList();

            }
            this.Db.Database.SetCommandTimeout(300);

            var partBooks = this.Db.Books
                                    .Include(x => x.SubCategory)
                                    .Where(x => x.ProductToShow == ProductToShowEnum.Yes && x.ReleaseDate.Date > DateTime.Today.Date)
                                    .OrderBy(x => x.ReleaseDate)
                                    .AsNoTracking();

            var result = partBooks.Select(b => new ProductPartDetailsViewModel
                {
                    ProductId = b.BookId,
                    Title = b.Title,
                    Author = b.Author,
                    Price = b.Price,
                    Publisher = b.Publisher,
                    ImageNameWithExtension = b.ImageNameWithExtension != "" ? $"{b.ReleaseDate.Year}/" + b.ImageNameWithExtension : "",
                    Discount = b.Discount,
                    ProductToShow = b.ProductToShow.ToString(),
                    SubcategoryId = b.SubCategory.SubcategoryId,
                    SubcategoryTitle = b.SubCategory.Title,
                    IsFavorite = userFavorites.Contains(b.BookId),
                    UrlTitle = ConvertCharsService.CyrillicToLatinChars(b.Title),
                    AuthorLatin = b.AuthorLatin,
                    PublisherLatin = b.PublisherLatin,
                    rating = (int)Math.Floor(b.Rating),
                    Date = b.ReleaseDate
                })
                .ToList();


            return result;
        }

        public IQueryable<ProductPartDetailsViewModel> ShowLastProducts(string userId)
        {
            var userFavorites = new List<int>();

            if (!string.IsNullOrEmpty(userId) || !string.IsNullOrWhiteSpace(userId))
            {
                userFavorites = this.Db.UserFavoriteProducts.Where(x => x.UserId == userId).AsNoTracking().Select(x => x.BookId).ToList();

            }
            this.Db.Database.SetCommandTimeout(300);
            var partBooks = this.Db.Books
                                    .Include(x => x.SubCategory)
                                    .Where(x => x.ProductToShow == ProductToShowEnum.Yes && x.ReleaseDate.Date <= DateTime.Today.Date && x.SubCategoryId < 58 || x.SubCategoryId > 69)
                                    .OrderByDescending(x => x.BookId)
                                    .Take(5000)
                                    .AsNoTracking();

            var result = partBooks.Select(b => new ProductPartDetailsViewModel
            {
                ProductId = b.BookId,
                Title = b.Title,
                Author = b.Author,
                Price = b.Price,
                Publisher = b.Publisher,
                ImageNameWithExtension = b.ImageNameWithExtension != "" ? $"{b.ReleaseDate.Year}/" + b.ImageNameWithExtension : "",
                Discount = b.Discount,
                ProductToShow = b.ProductToShow.ToString(),
                SubcategoryId = b.SubCategory.SubcategoryId,
                SubcategoryTitle = b.SubCategory.Title,
                IsFavorite = userFavorites.Contains(b.BookId),
                UrlTitle = ConvertCharsService.CyrillicToLatinChars(b.Title),
                AuthorLatin = b.AuthorLatin,
                PublisherLatin = b.PublisherLatin,
                rating = (int)Math.Floor(b.Rating),
                Date = b.ReleaseDate
            });


            return result;
        }

        public List<ProductPartDetailsViewModel> ShowProductsFromTeam(string userId)
        {
            this.Db.Database.SetCommandTimeout(300);
            var partBooks = this.Db.Books
                                    .Include(x => x.SubCategory)
                                    .Where(x => x.IsRecommendProduct == IsRecommendProductEnum.on)
                                    .AsNoTracking()
                                    .Select(b => new ProductPartDetailsViewModel
                                    {
                                        ProductId = b.BookId,
                                        Title = b.Title,
                                        Author = b.Author,
                                        Price = b.Price,
                                        Publisher = b.Publisher,
                                        ImageNameWithExtension = b.ImageNameWithExtension != "" ? $"{b.ReleaseDate.Year}/" + b.ImageNameWithExtension : "",
                                        Discount = b.Discount,
                                        ProductToShow = b.ProductToShow.ToString(),
                                        SubcategoryId = b.SubCategory.SubcategoryId,
                                        SubcategoryTitle = b.SubCategory.Title,
                                        CountOfOrders = b.CountOfOrders,
                                        UrlTitle = ConvertCharsService.CyrillicToLatinChars(b.Title),
                                        AuthorLatin = b.AuthorLatin,
                                        PublisherLatin = b.PublisherLatin,
                                        rating = (int)Math.Floor(b.Rating)
                                    })
                                    .OrderBy(x => x.CountOfOrders)
                                    .ToList();

            if (!string.IsNullOrEmpty(userId) || !string.IsNullOrWhiteSpace(userId))
            {
                var userFavorites = this.Db.UserFavoriteProducts.Where(x => x.UserId == userId).ToList();
                foreach (var b in partBooks)
                {
                    var isFavorite = userFavorites.FirstOrDefault(x => x.BookId == b.ProductId);
                    if (isFavorite != null)
                    {
                        b.IsFavorite = true;
                    }
                }
            }

            return partBooks;
        }

        public ProductDetailsViewModel AllProductDetails(int id)
        {
            var bookDetails = this.Db.Books.Include(x => x.SubCategory)
                                           .Where(b => b.BookId == id)
                                           .AsNoTracking()
                                           .Select(b => new ProductDetailsViewModel
                                           {
                                               ProductId = b.BookId,
                                               Title = b.Title,
                                               Author = b.Author,
                                               Price = b.Price,
                                               Discount = b.Discount,
                                               Pages = b.Pages,
                                               ImageName = b.ImageNameWithExtension != "" ? $"{b.ReleaseDate.Year}/" + b.ImageNameWithExtension : "",
                                               Format = b.Format,
                                               Publisher = b.Publisher,
                                               Cover = b.Cover.ToString(),
                                               ReleaseDate = b.ReleaseDate.ToString("dd-MM-yyyy"),
                                               ISBN = b.ISBN,
                                               Description = b.Description,
                                               SubcategoryId = b.SubCategory.SubcategoryId,
                                               SubcategoryTitle = b.SubCategory.Title,
                                               SubcategoryTitleLatin = ConvertCharsService.CyrillicToLatinChars(b.SubCategory.Title),
                                               AuthorLatin = b.AuthorLatin,
                                               PublisherLatin = b.PublisherLatin,
                                               ProductToShow = (int)b.ProductToShow,
                                               PriceWithDiscount = (b.Price * (100 - b.Discount)) / 100,
                                               Date = b.ReleaseDate.Date
                                           }).First();



            return bookDetails;
        }

        public List<ProductPartDetailsViewModel> SimilarProducts(int id, string userId)
        {
            var bookCategory = this.Db.Books.Include(x => x.SubCategory).First(b => b.BookId == id).SubCategory.Title;

            var partBooks = this.Db.Books
                                    .Where(b => b.SubCategory.Title == bookCategory && b.ProductToShow == ProductToShowEnum.Yes && b.BookId != id)
                                    .Include(x => x.SubCategory)
                                    .OrderByDescending(x => x.CountOfOrders)
                                    .ThenByDescending(x => x.BookId)
                                    .AsNoTracking()
                                    .Take(15)
                                    .Select(b => new ProductPartDetailsViewModel
                                    {
                                        ProductId = b.BookId,
                                        Title = b.Title,
                                        Author = b.Author,
                                        Price = b.Price,
                                        ImageNameWithExtension = b.ImageNameWithExtension != "" ? $"{b.ReleaseDate.Year}/" + b.ImageNameWithExtension : "",
                                        Publisher = b.Publisher,
                                        Discount = b.Discount,
                                        ProductToShow = b.ProductToShow.ToString(),
                                        SubcategoryId = b.SubCategory.SubcategoryId,
                                        SubcategoryTitle = b.SubCategory.Title,
                                        SubcategoryTitleLatin = ConvertCharsService.CyrillicToLatinChars(b.SubCategory.Title),
                                        UrlTitle = ConvertCharsService.CyrillicToLatinChars(b.Title),
                                        AuthorLatin = b.AuthorLatin,
                                        PublisherLatin = b.PublisherLatin,
                                        rating = (int)Math.Floor(b.Rating)
                                    }).ToList();

            if (!string.IsNullOrEmpty(userId) || !string.IsNullOrWhiteSpace(userId))
            {
                var userFavorites = this.Db.UserFavoriteProducts.Where(x => x.UserId == userId).ToList();
                foreach (var b in partBooks)
                {
                    var isFavorite = userFavorites.FirstOrDefault(x => x.BookId == b.ProductId);
                    if (isFavorite != null)
                    {
                        b.IsFavorite = true;
                    }
                }
            }

            if (partBooks.Count >= 5)
            {
                partBooks = partBooks.GetRange(0, 5).ToList();
            }
            else
            {
                partBooks = null;
            }


            return partBooks;
        }

        public ProductOrderIndexViewModel GetOrderIndexDetails(int id)
        {
            var details = this.Db.Books
                .Where(x => x.BookId == id)
                .AsNoTracking()
                .Select(x => new ProductOrderIndexViewModel
                {
                    Author = x.Author,
                    ImageName = $"{x.ReleaseDate.Year}/" + x.ImageNameWithExtension,
                    Publisher = x.Publisher,
                    Title = x.Title,
                    PriceWithDisc = ((x.Price * (100 - x.Discount)) / 100).ToString("f2")
                }).FirstOrDefault();

            return details;
        }

        public string SendMessageToServer(ContactFormViewModel model, string userId, MyIdentityUser user, bool isLogged)
        {
            var siteEmail = new SiteEmail
            {
                Name = model.Name,
                Email = model.Email,
                Message = model.Message,
                Phone = model.Phone,
                Subject = model.Subject,
                Date = DateTime.Today
            };

            if (isLogged)
            {
                siteEmail.UserId = userId;
                siteEmail.User = user;
            }


            string msg = "Съобщението беше изпратено успешно.";
            try
            {
                this.Db.SiteEmails.Add(siteEmail);
                this.Db.SaveChanges();
            }
            catch (Exception)
            {
                msg = "Съобщението не беше изпрате, моля опитайте пак.";
            }

            return msg;
        }

        public bool isProductFavorite(int bookId, string userId)
        {
            var user = this.Db.Users.Where(x => x.Id == userId).FirstOrDefault();
            var isFavorite = false;
            if (user != null)
            {
                var book = user.UserFavoriteProducts.Where(x => x.BookId == bookId).FirstOrDefault();
                if (book != null)
                {
                    isFavorite = true;
                }
            }

            return isFavorite;
        }

        public IQueryable<ProductPartDetailsViewModel> ProductsForSitemap(string userId, int productCount)
        {
            var userFavorites = new List<int>();

            if (!string.IsNullOrEmpty(userId) || !string.IsNullOrWhiteSpace(userId))
            {
                userFavorites = this.Db.UserFavoriteProducts.Where(x => x.UserId == userId).AsNoTracking().Select(x => x.BookId).ToList();

            }
            this.Db.Database.SetCommandTimeout(300);
            var partBooks = this.Db.Books
                                    .Include(x => x.SubCategory)
                                    .Where(x => x.ProductToShow == ProductToShowEnum.Yes)
                                    .OrderByDescending(x => x.BookId)
                                    .Take(productCount).AsNoTracking();

            var result = partBooks.Select(b => new ProductPartDetailsViewModel
            {
                ProductId = b.BookId,
                Title = b.Title,
                Author = b.Author,
                Price = b.Price,
                Publisher = b.Publisher,
                ImageNameWithExtension = b.ImageNameWithExtension != "" ? $"{b.ReleaseDate.Year}/" + b.ImageNameWithExtension : "",
                Discount = b.Discount,
                ProductToShow = b.ProductToShow.ToString(),
                SubcategoryId = b.SubCategory.SubcategoryId,
                SubcategoryTitle = b.SubCategory.Title,
                IsFavorite = userFavorites.Contains(b.BookId),
                UrlTitle = ConvertCharsService.CyrillicToLatinChars(b.Title),
                AuthorLatin = b.AuthorLatin,
                PublisherLatin = b.PublisherLatin,
                rating = (int)Math.Floor(b.Rating),
                Date = b.ReleaseDate
            });


            return result;
        }
    }
}
