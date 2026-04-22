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
    public class DutyShiftRepository : GenericRepository<DutyShift>, IDutyShiftRepository
    {
        public DutyShiftRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PaginatedList<DutyShift>> GetPagedDutyShiftsAsync(int pageIndex, int pageSize)
        {
            var query = _dbContext.DutyShifts
                .AsNoTracking();

            query = query.OrderBy(ds => ds.ShiftOrder);

            return await query.ToPaginatedListAsync(pageIndex, pageSize);
        }

        public async Task<IReadOnlyList<DutyShift>> GetAllShiftsAsync()
        {
            return await _dbContext.DutyShifts
                .AsNoTracking()
                .OrderBy(d => d.ShiftOrder)
                .ToListAsync();
        }

        public async Task<bool> HasAssignmentsAsync(Guid dutyShiftId)
        {
            return await _dbContext.Set<ShiftAssignment>().AnyAsync(d => d.DutyShiftId == dutyShiftId);
        }

        public async Task<bool> IsShiftOrderUniqueAsync(int shiftOrder, Guid? excludeId = null)
        {
            var query = _dbContext.DutyShifts.AsQueryable();

            if (excludeId.HasValue)
            {
                query = query.Where(m => m.Id != excludeId.Value);
            }

            return !await query.AnyAsync(m => m.ShiftOrder == shiftOrder);
        }
    }
}
