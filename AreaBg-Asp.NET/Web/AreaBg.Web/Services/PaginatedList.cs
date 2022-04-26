using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaBg.Web.Services
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalCount { get; private set; }
        public string Message { get; set; }
        public int CountOfProducts { get; set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize, int totalCount, string msg, int countProducts)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = totalCount;
            Message = msg;
            CountOfProducts = countProducts;
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

        public static PaginatedList<T> Create(IQueryable<T> source, int pageIndex, int pageSize, string msg, int countProducts)
        {
            var totalCount = source.Count();

            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageIndex, pageSize, totalCount, msg, countProducts);
        }
    }
}
