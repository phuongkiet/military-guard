using military_guard.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace military_guard.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(
            this IQueryable<T> source, int pageIndex, int pageSize) where T : class
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
