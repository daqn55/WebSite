using AreaBg.Data;
using AreaBg.Data.Models;
using AreaBg.Web.Areas.Admin.Books.ViewModels;
using AreaBg.Web.Areas.Admin.Services.Interfaces;
using AreaBg.Web.Areas.Admin.ViewModels;
using AreaBg.Web.Areas.Admin.ViewModels.Categories;
using AreaBg.Web.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.Areas.Admin.Services
{
    public class ProductAdminService : IProductAdminService
    {
        protected MyDbContext Db { get; }
        private readonly IHostEnvironment webHostEnvironment;

        public ProductAdminService(MyDbContext db, IHostEnvironment hostEnvironment)
        {
            this.Db = db;
            this.webHostEnvironment = hostEnvironment;
        }

        public AllCategoriesViewModel GetAllCategories()
        {
            var allCategoriesFromDb = this.Db.Categories.Include(x => x.Subcategories);
            var allCategories = new AllCategoriesViewModel();
            foreach (var c in allCategoriesFromDb)
            {
                var category = new CategoryViewModel
                {
                    CategoryId = c.CategoryId,
                    Title = c.Title,
                    OrderNumber = c.OrderNumber,
                    SubCategoriesViewModel = c.Subcategories.Select(x => new SubCategoryViewModel
                    {
                        SubCategoryId = x.SubcategoryId,
                        Title = x.Title,
                        OrderNumber = x.OrderNumber,
                        TitleLatin = ConvertCharsService.CyrillicToLatinChars(x.Title)
                    }).OrderBy(x => x.OrderNumber).ThenBy(x => x.Title).ToList()
                };
                allCategories.Categories.Add(category);
                allCategories.Categories = allCategories.Categories.OrderBy(x => x.OrderNumber).ThenBy(x => x.Title).ToList();
            }

            return allCategories;
        }

        public string MakeCategory(string categoryName)
        {
            string message = $"Категорията \"{categoryName}\", беше добавена успешно!";

            try
            {
                var category = new Category
                {
                    Title = categoryName
                };

                this.Db.Categories.Add(category);
                this.Db.SaveChanges();
            }
            catch (Exception e)
            {
                message = e.Message;
            }

            return message;
        }

        public string MakeSubCategory(string subCategoryName, string categoryNameWithId)
        {
            string message = string.Empty;

            try
            {
                int categoryId = int.Parse(categoryNameWithId.Split(": ")[0]);

                var category = this.Db.Categories.FirstOrDefault(x => x.CategoryId == categoryId);
                var subCategory = new Subcategory
                {
                    Title = subCategoryName,
                    Category = category
                };

                this.Db.Subcategories.Add(subCategory);
                this.Db.SaveChanges();

                message = $"Подкатегорията \"{subCategoryName}\", беше добавена успешно в категория \"{category.Title}\"!";
            }
            catch (Exception e)
            {
                message = e.Message;
            }

            return message;
        }

        public string AddProduct(ProductViewModel productViewModel)
        {
            string message = string.Empty;

            try
            {
                var subCat = this.Db.Subcategories.Find(productViewModel.CategorySelectCreateProduct);

                var book = new Book
                {
                    Title = productViewModel.ProductName,
                    Author = productViewModel.ProductAuthor,
                    Pages = productViewModel.ProductPages,
                    Format = productViewModel.ProductFormat,
                    Publisher = productViewModel.ProductPublisher,
                    Cover = productViewModel.ProductCover,
                    Price = productViewModel.ProductPrice,
                    Discount = productViewModel.ProductDiscount,
                    Weight = productViewModel.ProductWeight,
                    ProductToShow = productViewModel.ProductToShow,
                    ReleaseDate = productViewModel.ProductDate,
                    ISBN = productViewModel.ProductISBN,
                    BuyFrom = productViewModel.ProductBuyForm,
                    Description = productViewModel.ProductDescription,
                    SubCategory = subCat
                };

                this.Db.Books.Add(book);
                this.Db.SaveChanges();

                var b = this.Db.Books.Update(book);
                b.Entity.ImageNameWithExtension = UploadedFile(productViewModel, book.BookId, productViewModel.ProductDate.Year.ToString());

                this.Db.SaveChanges();

                

                message = $"Книгата \"{productViewModel.ProductName}\", беше добавена успешно в подкатегория \"{subCat.Title}\"!";
            }
            catch (Exception e)
            {
                message = e.Message;
            }

            return message;
        }

        private string UploadedFile(ProductViewModel model, int id, string year)
        {
            string uniqueFileName = null;

            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.ContentRootPath, $@"wwwroot\images\{year}");
                string typeOfFile = Path.GetExtension(model.Image.FileName);
                uniqueFileName = id.ToString() + typeOfFile;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        public BooksViewModel GetProductDetailsToEdit(int id)
        {
            var matchesByName = new BooksViewModel();

            try
            {
                var categories = GetAllCategories();
                matchesByName.Categories = categories.Categories;

                matchesByName.Book = this.Db.Books.Include(x => x.SubCategory).FirstOrDefault(x => x.BookId == id);


                return matchesByName;
            }
            catch (Exception)
            {
                //TODO: Make loger...
                return null;
            }
        }

        public SearchBooksViewModel SearchProduct(string productToSearch, string searchOption)
        {
            try
            {
                var searchBooks = new SearchBooksViewModel();

                switch (searchOption)
                {
                    case "byName":
                        searchBooks.PartBooks = this.Db.Books
                                           .Include(x => x.SubCategory.Category)
                                           .Where(b => b.Title.ToLower().Contains(productToSearch.Trim().ToLower()))
                                           .Select(b => new PartBookViewModel
                                                {
                                                    BookId = b.BookId,
                                                    Title = b.Title,
                                                    Publisher = b.Publisher,
                                                    SubCategory = b.SubCategory.Title,
                                                    ProductToShow = (int)b.ProductToShow
                                                })
                                           .OrderByDescending(x => x.BookId)
                                           .ToList();
                        break;
                    case "byAuthor":
                        searchBooks.PartBooks = this.Db.Books
                                           .Include(x => x.SubCategory.Category)
                                           .Where(b => b.Author.ToLower().Contains(productToSearch.Trim().ToLower()))
                                           .Select(b => new PartBookViewModel
                                           {
                                               BookId = b.BookId,
                                               Title = b.Title,
                                               Publisher = b.Publisher,
                                               SubCategory = b.SubCategory.Title,
                                               ProductToShow = (int)b.ProductToShow
                                           })
                                           .OrderByDescending(x => x.BookId)
                                           .ToList();
                        break;
                    case "byId":
                        searchBooks.PartBooks = this.Db.Books
                                           .Include(x => x.SubCategory.Category)
                                           .Where(b => b.BookId == int.Parse(productToSearch.Trim()))
                                           .Select(b => new PartBookViewModel
                                           {
                                               BookId = b.BookId,
                                               Title = b.Title,
                                               Publisher = b.Publisher,
                                               SubCategory = b.SubCategory.Title,
                                               ProductToShow = (int)b.ProductToShow
                                           })
                                           .OrderByDescending(x => x.BookId)
                                           .ToList();
                        break;
                    case "byPublisher":
                        searchBooks.PartBooks = this.Db.Books
                                           .Include(x => x.SubCategory.Category)
                                           .Where(b => b.Publisher.ToLower().Contains(productToSearch.Trim().ToLower()))
                                           .Select(b => new PartBookViewModel
                                           {
                                               BookId = b.BookId,
                                               Title = b.Title,
                                               Publisher = b.Publisher,
                                               SubCategory = b.SubCategory.Title,
                                               ProductToShow = (int)b.ProductToShow
                                           })
                                           .OrderByDescending(x => x.BookId)
                                           .ToList();
                        break;
                }

                return searchBooks;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string EditProduct(EditProductViewModel productViewModel)
        {
            string message = string.Empty;

            try
            {
                var subCat = this.Db.Subcategories.Find(productViewModel.CategorySelectCreateProduct);

                var book = new Book
                {
                    BookId = productViewModel.BookId,
                    Title = productViewModel.ProductName,
                    Author = productViewModel.ProductAuthor,
                    Pages = productViewModel.ProductPages,
                    Format = productViewModel.ProductFormat,
                    Publisher = productViewModel.ProductPublisher,
                    Cover = productViewModel.ProductCover,
                    Price = productViewModel.ProductPrice,
                    Discount = productViewModel.ProductDiscount,
                    Weight = productViewModel.ProductWeight,
                    ProductToShow = productViewModel.ProductToShow,
                    ReleaseDate = productViewModel.ProductDate,
                    ISBN = productViewModel.ProductISBN,
                    BuyFrom = productViewModel.ProductBuyForm,
                    Description = productViewModel.ProductDescription,
                    SubCategory = subCat,
                    IsRecommendProduct = productViewModel.RecommendedFromTeam,
                    OrderNumber = productViewModel.RecommendedProductPosition
                };

                var newImage = UploadedFileEdit(productViewModel, productViewModel.ProductDate.Year.ToString());
                if (newImage != null)
                {
                    book.ImageNameWithExtension = newImage;
                }
                else
                {
                    var imageName = this.Db.Books.AsNoTracking().First(x => x.BookId == productViewModel.BookId).ImageNameWithExtension;
                    if (imageName != null)
                    {
                        book.ImageNameWithExtension = imageName.ToString();
                    }
                    
                }

                this.Db.Books.Update(book);
                this.Db.SaveChanges();

                message = $"Книгата \"{productViewModel.ProductName}\", беше обновенна успешно!";
            }
            catch (Exception e)
            {
                message = e.Message;
            }

            return message;
        }

        private string UploadedFileEdit(EditProductViewModel model, string year)
        {
            string uniqueFileName = null;

            if (model.ImageNumber != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.ContentRootPath, $@"wwwroot\images\{year}");
                string typeOfFile = Path.GetExtension(model.ImageNumber.FileName);
                uniqueFileName = model.BookId.ToString() + typeOfFile;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImageNumber.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        public string UpdateCategoryOrder(IFormCollection orderNumbers)
        {
            foreach (var cat in orderNumbers)
            {
                var typeOfCat = cat.Key.Split('-')[0];

                if (typeOfCat == "cat")
                {
                    var categoryId = int.Parse(cat.Key.Split('-')[1]);
                    var orderValue = int.Parse(cat.Value);

                    var category = this.Db.Categories.Find(categoryId);
                    category.OrderNumber = orderValue;
                    this.Db.Categories.Update(category);
                }
                else if (typeOfCat == "subCat")
                {
                    var subCategoryId = int.Parse(cat.Key.Split('-')[1]);
                    var orderValue = int.Parse(cat.Value);

                    var subCategory = this.Db.Subcategories.Find(subCategoryId);
                    subCategory.OrderNumber = orderValue;
                    this.Db.Subcategories.Update(subCategory);
                }
            }

            this.Db.SaveChanges();

            return "";
        }
    }
}
