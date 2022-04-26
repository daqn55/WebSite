using AreaBg.Data;
using AreaBg.Web.Areas.Admin.ViewModels.Reviews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ReviewController : Controller
    {
        private MyDbContext Db { get; }

        public ReviewController(MyDbContext db)
        {
            this.Db = db;
        }

        public IActionResult Reviews()
        {
            var reviewsRaw = this.Db.BookReviews.Include(x => x.Book).AsEnumerable().Where(x => !x.IsApproved).ToList();

            var model = reviewsRaw
                .Select(x => new ReviewViewModel
                       { 
                            id = x.Id,
                            Subject = x.Subject,
                            Body = x.Body,
                            Date = x.date.ToString("yyyy-MM-dd"),
                            Name = x.Name,
                            Title = x.Book.Title
                       })
                .OrderByDescending(x => x.id)
                .ToList();

            return View(model);
        }

        public IActionResult RemoveReview(int id)
        {
            var review = this.Db.BookReviews.Find(id);

            this.Db.BookReviews.Remove(review);

            this.Db.SaveChanges();

            TempData["Success"] = "true";
            TempData["Message"] = "";

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult ApproveReview(int id)
        {
            var review = this.Db.BookReviews.Find(id);
            review.IsApproved = true;

            this.Db.BookReviews.Update(review);

            this.Db.SaveChanges();

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
