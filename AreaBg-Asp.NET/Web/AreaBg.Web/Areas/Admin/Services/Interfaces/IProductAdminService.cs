using AreaBg.Web.Areas.Admin.Books.ViewModels;
using AreaBg.Web.Areas.Admin.ViewModels;
using AreaBg.Web.Areas.Admin.ViewModels.Categories;
using Microsoft.AspNetCore.Http;

namespace AreaBg.Web.Areas.Admin.Services.Interfaces
{
    public interface IProductAdminService
    {
        AllCategoriesViewModel GetAllCategories();

        string MakeCategory(string categoryName);

        string MakeSubCategory(string subCategoryName, string categoryNameWithId);

        string AddProduct(ProductViewModel productViewModel);

        BooksViewModel GetProductDetailsToEdit(int id);

        SearchBooksViewModel SearchProduct(string productToSearch, string searchOption);

        string EditProduct(EditProductViewModel productViewModel);

        string UpdateCategoryOrder(IFormCollection orderNumbers);
    }
}
