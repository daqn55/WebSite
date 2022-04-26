using AreaBg.Data;
using AreaBg.Data.Models;
using AreaBg.Web.Areas.Admin.Services.Interfaces;
using AreaBg.Web.Areas.Admin.ViewModels;
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
    public class TermsController : Controller
    {
        protected MyDbContext Db { get; }

        public TermsController(MyDbContext db)
        {
            this.Db = db;
        }

        public IActionResult HelpAndTerms()
        {
            var helpAndTermsVM = new HelpAndTermsViewModel();

            var fromDbTerms = this.Db.HelpAndTerms.FirstOrDefault();

            if (fromDbTerms == null)
            {
                return View();
            }

            helpAndTermsVM.AboutUs = fromDbTerms.AboutUs;
            helpAndTermsVM.Delivery = fromDbTerms.DeliveryTerms;
            helpAndTermsVM.Terms = fromDbTerms.Terms;
            helpAndTermsVM.Confidentiality = fromDbTerms.Confidentiality;

            return View(helpAndTermsVM);
        }

        [HttpPost]
        public IActionResult UpdateAboutUs(string aboutUs)
        {
            TempData["Success"] = "true";
            TempData["Message"] = "\"За нас\" беше успешно променено.";

            var dbTerms = this.Db.HelpAndTerms.FirstOrDefault();

            if (dbTerms == null)
            {
                var helpAndTerm = new HelpAndTerms();
                helpAndTerm.AboutUs = aboutUs;
                this.Db.HelpAndTerms.Add(helpAndTerm);
            }
            else
            {
                dbTerms.AboutUs = aboutUs;
            }

            this.Db.SaveChanges();

            return Redirect("/Admin/Terms/HelpAndTerms");
        }

        [HttpPost]
        public IActionResult UpdateDelivery(string delivery)
        {
            var dbTerms = this.Db.HelpAndTerms.FirstOrDefault();

            if (dbTerms == null)
            {
                var helpAndTerm = new HelpAndTerms();
                helpAndTerm.DeliveryTerms = delivery;
                this.Db.HelpAndTerms.Add(helpAndTerm);
            }
            else
            {
                dbTerms.DeliveryTerms = delivery;
            }

            this.Db.SaveChanges();

            TempData["Success"] = "true";
            TempData["Message"] = "\"Доставката\" беше успешно променена.";

            return Redirect("/Admin/Terms/HelpAndTerms");
        }

        [HttpPost]
        public IActionResult UpdateTerms(string terms)
        {
            var dbTerms = this.Db.HelpAndTerms.FirstOrDefault();

            if (dbTerms == null)
            {
                var helpAndTerm = new HelpAndTerms();
                helpAndTerm.Terms = terms;
                this.Db.HelpAndTerms.Add(helpAndTerm);
            }
            else
            {
                dbTerms.Terms = terms;
            }

            this.Db.SaveChanges();

            TempData["Success"] = "true";
            TempData["Message"] = "\"Общите условия\" бяха успешно променени.";

            return Redirect("/Admin/Terms/HelpAndTerms");
        }

        [HttpPost]
        public IActionResult UpdateConfidentiality(string confidentiality)
        {
            var dbTerms = this.Db.HelpAndTerms.FirstOrDefault();

            if (dbTerms == null)
            {
                var helpAndTerm = new HelpAndTerms();
                helpAndTerm.Confidentiality = confidentiality;
                this.Db.HelpAndTerms.Add(helpAndTerm);
            }
            else
            {
                dbTerms.Confidentiality = confidentiality;
            }

            this.Db.SaveChanges();

            TempData["Success"] = "true";
            TempData["Message"] = "\"Политика за поверителност\" беше успешно променена.";

            return Redirect("/Admin/Terms/HelpAndTerms");
        }
    }
}
