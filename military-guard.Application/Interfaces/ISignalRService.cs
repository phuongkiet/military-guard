using military_guard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Interfaces
{
    public interface ISignalRService
    {
        Task SendAttendanceNotification(string militiaName, DateTime checkInTime, string status, bool isEmergency);
        Task SendCheckInEventToRoom(Guid dutyShiftId, DateOnly date, Guid militiaId, AttendanceStatus status, DateTime checkInTime);
        Task SendShiftStructureChangedEvent(Guid dutyShiftId, DateOnly date);
    }
}
