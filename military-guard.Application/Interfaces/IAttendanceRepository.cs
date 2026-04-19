using military_guard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Interfaces
{
    public interface IAttendanceRepository : IGenericRepository<Attendance>
    {
        Task<int> CountLateInMonth(Guid militiaId, DateTime month);
    }
}
