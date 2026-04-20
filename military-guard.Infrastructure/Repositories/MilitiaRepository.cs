using Microsoft.EntityFrameworkCore;
using military_guard.Application.Common.Models;
using military_guard.Application.Interfaces;
using military_guard.Domain.Entities;
using military_guard.Domain.Enums;
using military_guard.Infrastructure.Extensions;
using military_guard.Infrastructure.Persistence;

namespace military_guard.Infrastructure.Repositories;

public class MilitiaRepository : GenericRepository<Militia>, IMilitiaRepository
{
    public MilitiaRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<PaginatedList<Militia>> GetPagedMilitiasAsync(
        string? searchTerm, MilitiaType? type, MilitiaRank? rank, int pageIndex, int pageSize)
    {
        // Chú ý: Đổi thành _dbContext.Militias và BỎ Include(Department)
        var query = _dbContext.Militias
            .Include(m => m.Manager) // Chỉ cần Include người quản lý/chỉ huy
            .AsNoTracking();

        // 1. Lọc theo từ khóa tìm kiếm
        if (!string.IsNullOrWhiteSpace(searchTerm))
            query = query.Where(m => m.FullName.Contains(searchTerm) || m.Email.Contains(searchTerm));

        // 2. Lọc theo Phân loại (Thường trực / Cơ động)
        if (type.HasValue)
            query = query.Where(m => m.Type == type.Value);

        // 3. Lọc theo Cấp bậc (Lính / Tiểu đội phó...)
        if (rank.HasValue)
            query = query.Where(m => m.Rank == rank.Value);

        // Bắt buộc OrderBy trước khi gọi Extension Method phân trang
        query = query.OrderByDescending(m => m.Id);

        return await query.ToPaginatedListAsync(pageIndex, pageSize);
    }

    public async Task<Militia?> GetMilitiaDetailsAsync(Guid id)
    {
        return await _dbContext.Militias
            .Include(m => m.Manager)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<bool> IsEmailUniqueAsync(string email, Guid? excludeId = null)
    {
        var query = _dbContext.Militias.AsQueryable();

        if (excludeId.HasValue)
        {
            query = query.Where(m => m.Id != excludeId.Value);
        }

        return !await query.AnyAsync(m => m.Email == email);
    }

    public async Task<bool> IsValidManagerAsync(Guid managerId, Guid currentMilitiaId)
    {
        var manager = await _dbContext.Militias.FindAsync(managerId);

        if (manager == null)
            return false;

        if (manager.ManagerId == currentMilitiaId)
            return false;

        if (!manager.IsCapableLeader()) return false;

        return true;
    }
}