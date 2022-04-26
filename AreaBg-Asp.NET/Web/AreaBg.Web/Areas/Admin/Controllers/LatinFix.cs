using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AreaBg.Data;
using AreaBg.Data.Models;
using AreaBg.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AreaBg.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class LatinFix : Controller
    {
        private MyDbContext Db { get; set; }

        public LatinFix(MyDbContext db)
        {
            this.Db = db;
        }

        public IActionResult Fix()
        {
            return View();
        }

        public IActionResult FixLatin(string count)
        {
            var c = int.Parse(count);
            var books = new List<Book>();

            this.Db.Database.OpenConnection();

            if (c > 0)
            {
                books = this.Db.Books.OrderByDescending(x => x.BookId).Take(c).ToList();
            }
            else
            {
                books = this.Db.Books.Where(x => x.AuthorLatin == null && x.PublisherLatin == null).ToList();
            }

            foreach (var b in books)
            {
                var book = b;
                book.AuthorLatin = ConvertCharsService.CyrillicToLatinChars(book.Author);
                book.PublisherLatin = ConvertCharsService.CyrillicToLatinChars(book.Publisher);

                this.Db.Books.Update(book);

                this.Db.SaveChanges();

                Thread.Sleep(50);
            }

            this.Db.Database.CloseConnection();
            return View("fix");
        }

        [HttpPost]
        public async Task<IActionResult> CopyDiscount()
        {
            //var books = this.Db.Books;

            //foreach (var b in books)
            //{
            //    b.CopyOfDiscount = b.Discount;
            //}

            //await this.Db.SaveChangesAsync();

            return View("fix");
        }

        public async Task<IActionResult> RemoveDiscounts()
        {
            var books = this.Db.Books;

            foreach (var b in books)
            {
                b.Discount = 0;
            }

            await this.Db.SaveChangesAsync();

            return View("fix");
        }

        public async Task<IActionResult> PutDiscountsBack()
        {
            var books = this.Db.Books;

            foreach (var b in books)
            {
                b.Discount = b.CopyOfDiscount;
            }

            await this.Db.SaveChangesAsync();

            return View("fix");
        }
    }
}
