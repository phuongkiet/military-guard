using military_guard.Application.Common.Models;
using military_guard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Interfaces
{
    public interface IHolidayEventRepository : IGenericRepository<HolidayEvent>
    {
        Task<bool> IsHolidayEventUniqueAsync(string name, DateOnly date, Guid? excludeId = null);
        Task<PaginatedList<HolidayEvent>> GetPagedHolidyEventsAsync(int pageIndex, int pageSize);
    }
}
