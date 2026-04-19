using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Interfaces
{
    public interface ISignalRService
    {
        Task SendAttendanceNotification(string militiaName, DateTime checkInTime, string status, bool isEmergency);
    }
}
