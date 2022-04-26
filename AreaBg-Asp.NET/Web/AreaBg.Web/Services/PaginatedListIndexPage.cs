using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AreaBg.Web.ViewModels.CombinedViewModels;
using AreaBg.Web.ViewModels.Products;
using Microsoft.EntityFrameworkCore;

namespace AreaBg.Web.Services
{
    public class PaginatedListIndexPage<T> : List<ProductPartDetailsViewModel>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalCount { get; private set; }
        public List<ProductPartDetailsViewModel> ProductsFromTeam { get; set; }

        public List<ProductPartDetailsViewModel> UpcomingProducts { get; set; }
        public int CountLastProducts { get; set; }
        public int CountProductsFromTeam { get; set; }

        public PaginatedListIndexPage(List<ProductPartDetailsViewModel> items, int count, int pageIndex, int pageSize, int totalCount, List<ProductPartDetailsViewModel> productsFromTeam, int countLastProduct, int countProductFromTeam, List<ProductPartDetailsViewModel> upcomingProducts)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = totalCount;
            this.ProductsFromTeam = productsFromTeam;
            this.CountLastProducts = countLastProduct;
            this.CountProductsFromTeam = countProductFromTeam;
            this.UpcomingProducts = upcomingProducts;
            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public bool HasSecondNextPage 
        {
            get
            {
                return (PageIndex + 1 < TotalPages);
            }
        }
        public bool HasThirdNextPage
        {
            get
            {
                return (PageIndex + 2 < TotalPages);
            }
        }


        public static async Task<PaginatedListIndexPage<T>> CreateAsync(FirstPageViewModel source, int pageIndex, int pageSize)
        {
            var lastProducts = source.LastProducts;
            var productsFromTeam = source.ProductsFromTeam;
            var countLastProduct = source.CountLastProducts;
            var countProductFromTeam = source.CountProductsFromTeam;
            var upcomingProducts = source.UpcomingProducts;

            var totalCount = lastProducts.Count();

            var count = await lastProducts.CountAsync();
            var items = await lastProducts.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedListIndexPage<T>(items, count, pageIndex, pageSize, totalCount, productsFromTeam, countLastProduct, countProductFromTeam, upcomingProducts);
        }
    }
}
