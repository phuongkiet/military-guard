using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Common.Models
{
    public class PaginatedList<T>
    {
        public IReadOnlyCollection<T> Items { get; }
        public int PageIndex { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }

        public PaginatedList(IReadOnlyCollection<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Items = items;
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}
