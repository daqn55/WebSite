using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.ViewModels.Review
{
    public class NewReviewViewModel
    {
        public int bookId { get; set; }

        public string Date { get; set; }

        public string reviewName { get; set; }

        public string reviewSubject { get; set; }

        public string reviewBody { get; set; }

        public int stars { get; set; }

        public string captcha { get; set; }
    }
}
