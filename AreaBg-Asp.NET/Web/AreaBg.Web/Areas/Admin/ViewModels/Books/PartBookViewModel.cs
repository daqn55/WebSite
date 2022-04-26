using AreaBg.Data.Models.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.Areas.Admin.Books.ViewModels
{
    public class PartBookViewModel
    {
        public int BookId { get; set; }

        public string Title { get; set; }

        public string SubCategory { get; set; }

        public string Publisher { get; set; }

        public int ProductToShow { get; set; }
    }
}
