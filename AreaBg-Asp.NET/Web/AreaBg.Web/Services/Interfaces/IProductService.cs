using AreaBg.Data.Models;
using AreaBg.Web.ViewModels.Contact;
using AreaBg.Web.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.Interfaces.Services
{
    public interface IProductService
    {
        List<ProductPartDetailsViewModel> UpcomingProducts(string userId);

        IQueryable<ProductPartDetailsViewModel> ShowLastProducts(string userId);
        
        List<ProductPartDetailsViewModel> ShowProductsFromTeam(string userId);

        ProductDetailsViewModel AllProductDetails(int id);

        List<ProductPartDetailsViewModel> SimilarProducts(int id, string userId);

        ProductOrderIndexViewModel GetOrderIndexDetails(int id);

        string SendMessageToServer(ContactFormViewModel model, string userId, MyIdentityUser user, bool isLogged);

        bool isProductFavorite(int bookId, string userId);

        IQueryable<ProductPartDetailsViewModel> ProductsForSitemap(string userId, int productCount);
    }
}
