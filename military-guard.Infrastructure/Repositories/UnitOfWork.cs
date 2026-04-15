using military_guard.Application.Interfaces;
using military_guard.Infrastructure.Persistence;

namespace military_guard.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Chỗ này chính là nơi ra lệnh cho EF Core đẩy dữ liệu xuống SQL Server
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
