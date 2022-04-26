using AreaBg.Data;
using AreaBg.Web.Areas.Admin.ViewModels.JsonRedact;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AreaBg.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class JsonRedactController : Controller
    {
        private MyDbContext Db { get; set; }

        public JsonRedactController(MyDbContext db)
        {
            this.Db = db;
        }

        public IActionResult Redact()
        {
            return View();
        }

        public IActionResult NewPrice(string nameOfFile)
        {
            var json = System.IO.File.ReadAllText(@$"wwwroot/jsonRedact/{nameOfFile}.json").Trim();
            var msg = "Книгите с нови цени бяха успешно записани.";
            try
            {
                var booksModel = JsonSerializer.Deserialize<IEnumerable<NewPriceViewModel>>(json).ToList();

                foreach (var b in booksModel)
                {
                    var book = this.Db.Books.Find(int.Parse(b.BookId));
                    book.Price = decimal.Parse(b.Price);
                    book.ImageNameWithExtension = b.BookId + ".jpg";
                    this.Db.Update(book);
                    this.Db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
            }

            TempData["Success"] = "true";
            TempData["Message"] = msg;

            return View("Redact");
        }

        public IActionResult OnlyPic(string nameOfFile)
        {
            var json = System.IO.File.ReadAllText(@$"wwwroot/jsonRedact/{nameOfFile}.json").Trim();
            var msg = "Добавени са успешно имената за кориците.";
            try
            {
                var booksModel = JsonSerializer.Deserialize<IEnumerable<NewPriceViewModel>>(json).ToList();

                foreach (var b in booksModel)
                {
                    var book = this.Db.Books.Find(int.Parse(b.BookId));
                    book.ImageNameWithExtension = b.BookId + ".jpg";
                    this.Db.Update(book);
                    this.Db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
            }

            TempData["Success"] = "true";
            TempData["Message"] = msg;

            return View("Redact");
        }

        public IActionResult ToRemove(string nameOfFile)
        {
            var json = System.IO.File.ReadAllText(@$"wwwroot/jsonRedact/{nameOfFile}.json").Trim();
            var msg = "Книгите са успешно премахнати.";
            try
            {
                var booksModel = JsonSerializer.Deserialize<IEnumerable<NewPriceViewModel>>(json).ToList();

                foreach (var b in booksModel)
                {
                    var book = this.Db.Books.Find(int.Parse(b.BookId));
                    book.ProductToShow = Data.Models.enums.ProductToShowEnum.No;
                    this.Db.Update(book);
                    this.Db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
            }

            TempData["Success"] = "true";
            TempData["Message"] = msg;

            return View("Redact");
        }
    }
}
