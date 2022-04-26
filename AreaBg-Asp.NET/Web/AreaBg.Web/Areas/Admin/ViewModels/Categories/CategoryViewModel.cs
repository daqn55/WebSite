using System.Collections.Generic;

namespace AreaBg.Web.Areas.Admin.ViewModels.Categories
{
    public class CategoryViewModel
    {
        public CategoryViewModel()
        {
            this.SubCategoriesViewModel = new List<SubCategoryViewModel>();
        }

        public int CategoryId { get; set; }

        public string Title { get; set; }

        public int OrderNumber { get; set; }

        public List<SubCategoryViewModel> SubCategoriesViewModel { get; set; }
    }
}
