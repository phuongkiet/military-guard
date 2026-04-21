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
    public class ShiftAssignmentRepository : GenericRepository<ShiftAssignment>, IShiftAssignmentRepository
    {
        public ShiftAssignmentRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<int> CountMilitiasInShiftAsync(Guid guardPostId, Guid dutyShiftId, DateOnly date)
        {
            return await _dbContext.ShiftAssignments.Where(sa => sa.GuardPostId == guardPostId && sa.DutyShiftId == dutyShiftId && sa.Date == date).CountAsync();
        }

        public async Task<PaginatedList<ShiftAssignment>> GetPagedAssignmentsAsync(DateOnly? date, Guid? guardPostId, Guid? militiaId, int pageIndex, int pageSize)
        {
            var query = _dbContext.ShiftAssignments
                .Include(g => g.GuardPost)
                .Include(m => m.Militia)
                .Include(d => d.DutyShift)
                .AsNoTracking();

            var targetDate = date ?? DateOnly.FromDateTime(DateTime.Now);

            query = query.Where(sa => sa.Date == targetDate);

            if (date.HasValue)
            {
                query = query.Where(sa => sa.Date == date);
            }

            if (guardPostId.HasValue)
            {
                query = query.Where(sa => sa.GuardPostId == guardPostId);
            }

            if (militiaId.HasValue)
            {
                query = query.Where(sa => sa.MilitiaId == militiaId);
            }

            query = query.OrderBy(sa => sa.Date)
             .ThenBy(sa => sa.DutyShift.ShiftOrder)
             .ThenBy(sa => sa.GuardPost.Name);

            return await query.ToPaginatedListAsync(pageIndex, pageSize);
        }

        public async Task<ShiftAssignment?> GetAssignmentDetailsAsync(Guid id)
        {
            return await _dbContext.ShiftAssignments
                .Include(sa => sa.Militia)
                .Include(sa => sa.GuardPost)
                .Include(sa => sa.DutyShift)
                .AsNoTracking()
                .FirstOrDefaultAsync(sa => sa.Id == id);
        }

        public async Task<bool> IsMilitiaDoubleBookedAsync(Guid militiaId, DateOnly date, Guid dutyShiftId)
        {
            return await _dbContext.ShiftAssignments
                .AnyAsync(sa => sa.MilitiaId == militiaId && sa.Date == date && sa.DutyShiftId == dutyShiftId);
        }
    }
}
