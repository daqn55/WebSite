using AreaBg.Data.Models;
using AreaBg.Web.Areas.Admin.ViewModels.Categories;
using System.Collections.Generic;

namespace AreaBg.Web.Areas.Admin.Books.ViewModels
{
    public class BooksViewModel
    {
        public BooksViewModel()
        {
            this.Categories = new List<CategoryViewModel>();
        }

        public Book Book { get; set; }

        public List<CategoryViewModel> Categories { get; set; }
    }
}
