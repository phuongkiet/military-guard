using military_guard.Application.Common.Models;
using military_guard.Application.Features.ShiftAssignments.DTOs;
using military_guard.Domain.Entities;
using military_guard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Interfaces
{
    public interface IMilitiaRepository : IGenericRepository<Militia>
    {
        Task<PaginatedList<Militia>> GetPagedMilitiasAsync(
        string? searchTerm, MilitiaType? type, MilitiaRank? rank, int pageIndex, int pageSize);
        Task<Militia?> GetMilitiaDetailsAsync(Guid id);
        Task<bool> IsEmailUniqueAsync(string email, Guid? excludeId = null);
        Task<bool> IsValidManagerAsync(Guid managerId, Guid currentEmployeeId);
        Task<List<SubstituteDto>> GetAvailableForShiftAsync(Guid shiftId, DateOnly date);
    }
}
