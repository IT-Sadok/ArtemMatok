using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Response
{
    public class PageResultResponse<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPage => (int)Math.Ceiling((double)TotalCount / PageSize);

        public bool HasNextPage => CurrentPage < TotalPage;
        public bool HasPreviousPage => CurrentPage > 1;

        public PageResultResponse(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = count;
            CurrentPage = pageNumber;
            PageSize = pageSize;
        }
    }
}
