using MediatR;
using military_guard.Application.Common.Models;
using military_guard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Interfaces
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<PaginatedList<Account>> GetPagedAccountsAsync(string? searchTerm, bool? isActive, int pageIndex, int pageSize);
        Task<Account?> GetByUsernameAsync(string username);
        Task<bool> IsUsernameUniqueAsync(string username);
        Task BanUser(Guid userId);
        Task UnbanUser(Guid userId);
    }
}
