using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
