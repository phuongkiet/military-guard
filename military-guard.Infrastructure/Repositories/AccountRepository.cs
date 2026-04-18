using Microsoft.EntityFrameworkCore;
using military_guard.Application.Common.Models;
using military_guard.Application.Interfaces;
using military_guard.Domain.Entities;
using military_guard.Infrastructure.Extensions;
using military_guard.Infrastructure.Persistence;

namespace military_guard.Infrastructure.Repositories
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task BanUser(Guid userId)
        {
            var user = await _dbContext.Accounts.FirstOrDefaultAsync(u => u.Id == userId);
            if(user != null)
            {
                user.IsBanned = true;
            }
        }

        public async Task<Account?> GetByUsernameAsync(string username)
        {
            return await _dbContext.Accounts.Include(m => m.Militia).AsNoTracking().FirstOrDefaultAsync(a => a.Username == username);
        }

        public async Task<PaginatedList<Account>> GetPagedAccountsAsync(string? searchTerm, bool? isActive, int pageIndex, int pageSize)
        {
            var query = _dbContext.Accounts.AsNoTracking();

            // 1. Lọc theo từ khóa tìm kiếm
            if (!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(m => m.Username.Contains(searchTerm));

            if (isActive.HasValue)
            {
                query = query.Where(m => m.IsBanned == isActive);
            }

            query = query.OrderByDescending(m => m.Id);

            return await query.ToPaginatedListAsync(pageIndex, pageSize);
        }

        public async Task<bool> IsUsernameUniqueAsync(string username)
        {
            return !await _dbContext.Accounts.AnyAsync(a => a.Username == username);
        }

        public async Task UnbanUser(Guid userId)
        {
            var user = await _dbContext.Accounts.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                user.IsBanned = false;
            }
        }
    }
}
