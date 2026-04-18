using military_guard.Domain.Entities;

namespace military_guard.Application.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(Account account);
    }
}
