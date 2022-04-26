using AreaBg.Data;
using AreaBg.Web.Areas.Admin.ViewModels.Mails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MailsController : Controller
    {
        protected MyDbContext Db { get; }

        public MailsController(MyDbContext db)
        {
            this.Db = db;
        }


        public IActionResult ReadMails(string mailsView)
        {
            var isArchived = false;

            if (mailsView == "archived")
            {
                isArchived = true;
            }

            var mailsFromDb = this.Db.SiteEmails
                .Where(x => x.Archived == isArchived)
                .AsEnumerable()
                .ToList();

            var mails = mailsFromDb.Select(x => new MailsViewModel
            {
                Date = x.Date.ToString("yyyy-MM-dd"),
                Email = x.Email,
                id = x.Id,
                Message = x.Message,
                Name = x.Name,
                Phone = x.Phone,
                Subject = x.Subject,
                Archived = x.Archived
            }).ToList();

            return View(mails);
        }

        public IActionResult RemoveMail(int id)
        {
            var mail = this.Db.SiteEmails.Find(id);
            
            this.Db.SiteEmails.Remove(mail);

            this.Db.SaveChanges();

            TempData["Success"] = "true";
            TempData["Message"] = "";

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult ArchiveMail(int id)
        {
            var mail = this.Db.SiteEmails.Find(id);
            mail.Archived = true;

            this.Db.SiteEmails.Update(mail);

            this.Db.SaveChanges();

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
