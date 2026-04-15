using military_guard.Application.Common.Models;
using military_guard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Interfaces
{
    public interface IGuardPostRepository : IGenericRepository<GuardPost>
    {
        Task<PaginatedList<GuardPost>> GetPagedGuardPostsAsync(string? searchTerm, bool? isActive, int pageIndex, int pageSize);

        Task<bool> IsNameUniqueAsync(string name, Guid? excludeId = null);

        Task<bool> HasAssignmentsAsync(Guid guardPostId);
    }
}
