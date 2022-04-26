using System.Collections.Generic;

namespace AreaBg.Web.Areas.Admin.ViewModels.Categories
{
    public class AllCategoriesViewModel
    {
        public AllCategoriesViewModel()
        {
            this.Categories = new List<CategoryViewModel>();
        }

        public List<CategoryViewModel> Categories { get; set; }
    }
}
