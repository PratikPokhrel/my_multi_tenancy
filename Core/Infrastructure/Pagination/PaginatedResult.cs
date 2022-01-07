using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Pagination
{
    public class PaginatedResult<T> 
    {
        public PaginatedResult(List<T> items)
        {
            Items = items;
        }

        public List<T> Items { get; set; }

        public PaginatedResult(List<T> items = default, int count = 0, int page = 1, int pageSize = 10)
        {
            Items = items;
            CurrentPage = page;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
        }

        public static PaginatedResult<T> Success(List<T> data, int count, int page, int pageSize)
        {
            return new PaginatedResult<T>(data, count, page, pageSize);
        }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int TotalCount { get; set; }
        public int PageSize { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;

        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
