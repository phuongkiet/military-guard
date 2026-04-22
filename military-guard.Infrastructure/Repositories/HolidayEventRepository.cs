using Microsoft.EntityFrameworkCore;
using military_guard.Application.Common.Models;
using military_guard.Application.Interfaces;
using military_guard.Domain.Entities;
using military_guard.Infrastructure.Extensions;
using military_guard.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Infrastructure.Repositories
{
    public class HolidayEventRepository : GenericRepository<HolidayEvent>, IHolidayEventRepository
    {
        public HolidayEventRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PaginatedList<HolidayEvent>> GetPagedHolidyEventsAsync(int pageIndex, int pageSize)
        {
            var query = _dbContext.HolidayEvents
                .AsNoTracking();

            query = query.OrderBy(ds => ds.Date);

            return await query.ToPaginatedListAsync(pageIndex, pageSize);
        }

        public async Task<bool> IsHolidayEventUniqueAsync(string name, DateOnly date, Guid? excludeId = null)
        {
            var query = _dbContext.HolidayEvents.AsQueryable();

            if (excludeId.HasValue)
            {
                query = query.Where(m => m.Id != excludeId.Value);
            }

            return !await query.AnyAsync(m => m.Name == name || m.Date == date);
        }
    }
}
