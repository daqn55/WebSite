using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.Areas.Admin.ViewModels.Reviews
{
    public class ReviewViewModel
    {
        public int id { get; set; }

        public string Date { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}
