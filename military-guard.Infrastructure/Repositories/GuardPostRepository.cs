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
    public class GuardPostRepository : GenericRepository<GuardPost>, IGuardPostRepository
    {
        public GuardPostRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PaginatedList<GuardPost>> GetPagedGuardPostsAsync(string? searchTerm, bool? isActive, int pageIndex, int pageSize)
        {
            var query = _dbContext.GuardPosts.AsNoTracking();

            // 1. Lọc theo từ khóa tìm kiếm
            if (!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(m => m.Name.Contains(searchTerm));

            if (isActive.HasValue)
            {
                query = query.Where(m => m.IsActive == isActive);
            }

            // Bắt buộc OrderBy trước khi gọi Extension Method phân trang
            query = query.OrderByDescending(m => m.Id);

            return await query.ToPaginatedListAsync(pageIndex, pageSize);
        }

        public async Task<bool> HasAssignmentsAsync(Guid guardPostId)
        {
            return await _dbContext.Set<ShiftAssignment>()
                           .AnyAsync(sa => sa.GuardPostId == guardPostId);
        }

        public async Task<bool> IsNameUniqueAsync(string name, Guid? excludeId = null)
        {
            var query = _dbContext.GuardPosts.AsQueryable();

            if (excludeId.HasValue)
            {
                query = query.Where(m => m.Id != excludeId.Value);
            }

            return !await query.AnyAsync(m => m.Name == name);
        }
    }
}
