using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AreaBg.Data;
using AreaBg.Data.Models;
using AreaBg.Data.Models.enums;
using AreaBg.Web.Areas.Admin.ViewModels.Books;
using AreaBg.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AreaBg.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class JsonController : Controller
    {
        private MyDbContext Db { get; set; }

        public JsonController(MyDbContext db)
        {
            this.Db = db;
        }

        //[HttpGet("Admin/Json/Index")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddJson(string nameOfFile, string JsonString)
        {
            var msg = string.Empty;
            if (nameOfFile != null)
            {
                msg = $"Успешно добави книгите от файла - {nameOfFile}";
            }

            var json = string.Empty;
            if (JsonString != null)
            {
                msg = $"Успешно добави книгите";
                json = JsonString.Trim();
            }
            else
            {
                json = System.IO.File.ReadAllText(@$"wwwroot/json/{nameOfFile}.json").Trim();
            }

            var booksModel = JsonSerializer.Deserialize<IEnumerable<BookJsonViewModel>>(json);

            foreach (var book in booksModel)
            {
                if (book.vis == "yes")
                {
                    if (book.subcatID != "")
                    {
                        var aa = new Dictionary<string, int>
                            {
                                {"1", 49}, {"2", 50}, {"4", 43}, {"5", 51}, {"6", 16}, {"7", 41},
                                {"8", 3}, {"9", 51}, {"10", 30}, {"14", 56}, {"15", 44}, {"16", 48},
                                {"17", 45}, {"18", 18}, {"19", 20}, {"20", 21}, {"21", 22}, {"22", 24},
                                {"23", 25}, {"24", 26}, {"25", 27}, {"26", 28}, {"27", 29}, {"28" ,4},
                                {"29", 1}, {"30", 6}, {"31", 8}, {"32", 9}, {"33", 12}, {"34", 13},
                                {"35", 14}, {"36", 15}, {"37", 37}, {"38", 40}, {"39", 17}, {"40", 31},
                                {"41", 38}, {"42", 36}, {"43", 5}, {"44", 32}, {"45", 19}, {"48", 33},
                                {"50", 46}, {"51", 47}, {"52", 55}, {"53", 23}, {"57", 57}, {"58", 54},
                                {"61", 42}, {"62", 39}, {"63", 7}, {"68", 11}, {"74", 53}
                            };

                        aa.TryGetValue(book.subcatID, out int subcatId);
                        if (subcatId != 0)
                        {
                            DateTime.TryParse(book.date, out DateTime date);
                            AddBookToDatabase(book, subcatId, date);
                        }
                    }
                    else
                    {
                        AddBookToDatabase(book, int.Parse(book.subcatIDArea), DateTime.Today);
                    }


                }
            }

            var books = this.Db.Books;

            this.Db.Database.OpenConnection();
            try
            {
                this.Db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Books ON;");
                this.Db.SaveChanges();
                this.Db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Books OFF;");
            }
            catch (Exception е)
            {
                msg = е.InnerException.Message;
            }
            finally
            {
                this.Db.Database.CloseConnection();
            }

            TempData["Success"] = "true";
            TempData["Message"] = msg;

            return Redirect("/Admin/Json/Index");
        }

        private void AddBookToDatabase(BookJsonViewModel book, int subcatId, DateTime date)
        {
            var title = WebUtility.HtmlDecode(book.title);
            var cover = ProductCoverEnum.SoftCover;
            if (book.cover.ToLower().Trim() == "твърда")
            {
                cover = ProductCoverEnum.HardCover;
            }

            var description = "";
            if (string.IsNullOrEmpty(book.description) || string.IsNullOrWhiteSpace(book.description))
            {
                description = WebUtility.HtmlDecode(book.partndescrip);
            }
            else
            {
                description = WebUtility.HtmlDecode(book.description);
            }
            description = description.Replace("<p>", "");
            description = description.Replace("<br>", "");
            description = description.Replace("<p align=right>", "");
            description = description.Replace("<p align=left>", "");
            description = description.Replace("<p align=center>", "");

            int.TryParse(book.pages, out int pages);
            double.TryParse(book.price2, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double discount);
            int.TryParse(book.weight, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out int weight);
            decimal.TryParse(book.price1, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal price);
            

            var subcat = this.Db.Subcategories.Find(subcatId);
            var newBook = new Book
            {
                BookId = int.Parse(book.ID),
                Title = title,
                Author = book.author,
                Pages = pages,
                Format = book.format,
                Publisher = book.publisher,
                Cover = cover,
                Price = price,
                Discount = (int)discount,
                ImageNameWithExtension = book.image,
                Weight = weight,
                ProductToShow = ProductToShowEnum.Yes,
                IsRecommendProduct = IsRecommendProductEnum.off,
                ReleaseDate = date,
                ISBN = book.isbn,
                BuyFrom = book.kupeno,
                Description = description,
                SubCategory = subcat,
                AuthorLatin = ConvertCharsService.CyrillicToLatinChars(book.author),
                PublisherLatin = ConvertCharsService.CyrillicToLatinChars(book.publisher),
                CopyOfDiscount = (int)discount
            };

            this.Db.Books.Add(newBook);
        }
    }
}
