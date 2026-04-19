using Microsoft.AspNetCore.SignalR;
using military_guard.Application.Interfaces;
using military_guard.Infrastructure.SignalRHubs;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Infrastructure.Services
{
    public class SignalRService : ISignalRService
    {
        private readonly IHubContext<AttendanceHub> _hubContext;

        public SignalRService(IHubContext<AttendanceHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendAttendanceNotification(string militiaName, DateTime checkInTime, string status, bool isEmergency)
        {
            await _hubContext.Clients.Group("Commanders").SendAsync("ReceiveAttendanceUpdate", new
            {
                MilitiaName = militiaName,
                CheckInTime = checkInTime.ToString("HH:mm:ss"),
                Status = status,
                IsEmergency = isEmergency
            });
        }
    }
}
