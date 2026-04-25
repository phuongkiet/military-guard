using military_guard.Application.Common.Models;
using military_guard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Interfaces
{
    public interface IShiftAssignmentRepository : IGenericRepository<ShiftAssignment>
    {
        Task<PaginatedList<ShiftAssignment>> GetPagedAssignmentsAsync(
        DateOnly? date, Guid? guardPostId, Guid? militiaId, int pageIndex, int pageSize);

        Task<ShiftAssignment?> GetAssignmentDetailsAsync(Guid id);

        Task<List<ShiftAssignment>> GetAssignmentsByShiftAndDateAsync(Guid shiftId, DateOnly date);

        Task<bool> IsMilitiaDoubleBookedAsync(Guid militiaId, DateOnly date, Guid dutyShiftId);

        Task<int> CountMilitiasInShiftAsync(Guid guardPostId, Guid dutyShiftId, DateOnly date);
    }
}
