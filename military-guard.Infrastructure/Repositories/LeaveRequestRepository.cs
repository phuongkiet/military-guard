using Microsoft.EntityFrameworkCore;
using military_guard.Application.Common.Models;
using military_guard.Application.Interfaces;
using military_guard.Domain.Entities;
using military_guard.Domain.Enums;
using military_guard.Infrastructure.Extensions;
using military_guard.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Infrastructure.Repositories
{
    public class LeaveRequestRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
    {
        public LeaveRequestRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PaginatedList<LeaveRequest>> GetPagedLeaveRequestsAsync(Guid? militiaId, LeaveStatus? status, int pageIndex, int pageSize)
        {
            var query = _dbContext.LeaveRequests
                .Include(l => l.Militia)
                .AsNoTracking();

            if (militiaId.HasValue)
                query = query.Where(l => l.MilitiaId == militiaId);

            if (status.HasValue)
                query = query.Where(l => l.Status == status);

            // Đơn mới nhất lên đầu
            query = query.OrderByDescending(l => l.CreatedAt);

            return await query.ToPaginatedListAsync(pageIndex, pageSize);
        }

        public async Task<LeaveRequest?> GetLeaveRequestDetailsAsync(Guid id)
        {
            return await _dbContext.LeaveRequests
                .Include(l => l.Militia)
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<bool> HasOverlappingLeaveAsync(Guid militiaId, DateTime startDate, DateTime endDate, Guid? excludeId = null)
        {
            var query = _dbContext.LeaveRequests.AsQueryable();

            if (excludeId.HasValue)
            {
                query = query.Where(l => l.Id != excludeId.Value);
            }

            return await query.AnyAsync(l =>
                l.MilitiaId == militiaId &&
                l.Status != LeaveStatus.Rejected &&
                startDate <= l.EndDate &&
                endDate >= l.StartDate);
        }
    }
}
