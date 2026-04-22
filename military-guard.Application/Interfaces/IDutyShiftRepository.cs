using military_guard.Application.Common.Models;
using military_guard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Interfaces
{
    public interface IDutyShiftRepository : IGenericRepository<DutyShift>
    {
        Task<IReadOnlyList<DutyShift>> GetAllShiftsAsync();
        Task<PaginatedList<DutyShift>> GetPagedDutyShiftsAsync(int pageIndex, int pageSize);

        Task<bool> IsShiftOrderUniqueAsync(int shiftOrder, Guid? excludeId = null);

        Task<bool> HasAssignmentsAsync(Guid dutyShiftId);
    }
}
