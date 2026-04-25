using Microsoft.EntityFrameworkCore;
using military_guard.Application.Interfaces;
using military_guard.Domain.Entities;
using military_guard.Domain.Enums;
using military_guard.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace military_guard.Infrastructure.Repositories
{
    public class AttendanceRepository : GenericRepository<Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<int> CountLateInMonth(Guid militiaId, DateTime month)
        {
            var startOfMonth = new DateTime(month.Year, month.Month, 1);
            var startOfNextMonth = startOfMonth.AddMonths(1);

            return await _dbContext.Attendances
                .CountAsync(a =>
                    a.MilitiaId == militiaId &&
                    a.Status == AttendanceStatus.LateWarning &&
                    a.CheckInTime >= startOfMonth &&
                    a.CheckInTime < startOfNextMonth
                );
        }

        public async Task<bool> HasCheckedInAsync(Guid militiaId, Guid shiftId, DateOnly date)
        {
            return await _dbContext.Attendances
                .AnyAsync(a => a.MilitiaId == militiaId
                            && a.ShiftId == shiftId
                            && a.CheckInTime.Year == date.Year
                            && a.CheckInTime.Month == date.Month
                            && a.CheckInTime.Day == date.Day);
        }

        public async Task<List<Attendance>> GetAttendancesByShiftAndDateAsync(Guid shiftId, DateOnly date)
        {
            return await _dbContext.Attendances
                .Where(a => a.ShiftId == shiftId && a.CheckInTime.Date == date.ToDateTime(TimeOnly.MinValue).Date)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
