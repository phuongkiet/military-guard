using military_guard.Application.Common.Models;
using military_guard.Domain.Entities;
using military_guard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Interfaces
{
    public interface ILeaveRequestRepository : IGenericRepository<LeaveRequest>
    {
        Task<PaginatedList<LeaveRequest>> GetPagedLeaveRequestsAsync(Guid? militiaId, LeaveStatus? status, int pageIndex, int pageSize);
        Task<LeaveRequest?> GetLeaveRequestDetailsAsync(Guid id);
        Task<bool> HasOverlappingLeaveAsync(Guid militiaId, DateTime startDate, DateTime endDate, Guid? excludeId = null);
    }
}
