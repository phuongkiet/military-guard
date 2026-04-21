using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Interfaces
{
    public interface IShiftSchedulingService
    {
        Task GenerateWeeklyScheduleAsync();

        Task ScanAndNotifyUpcomingShiftsAsync();
    }
}
