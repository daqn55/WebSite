using System.Collections.Generic;

namespace AreaBg.Web.Areas.Admin.Books.ViewModels
{
    public class SearchBooksViewModel
    {
        public SearchBooksViewModel()
        {
            this.PartBooks = new List<PartBookViewModel>();
        }

        public List<PartBookViewModel> PartBooks { get; set; }
    }
}
