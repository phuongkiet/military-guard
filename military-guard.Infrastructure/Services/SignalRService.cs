using Microsoft.AspNetCore.SignalR;
using military_guard.Application.Interfaces;
using military_guard.Domain.Enums;
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

        public async Task SendCheckInEventToRoom(Guid dutyShiftId, DateOnly date, Guid militiaId, AttendanceStatus status, DateTime checkInTime)
        {
            string roomName = $"LiveShift_{date:yyyyMMdd}_{dutyShiftId.ToString().ToLower()}";

            await _hubContext.Clients.Group(roomName).SendAsync("ReceiveAttendanceUpdate", new
            {
                militiaId = militiaId,
                status = status,
                checkInTime = checkInTime
            });
        }

        public async Task SendShiftStructureChangedEvent(Guid dutyShiftId, DateOnly date)
        {
            string roomName = $"LiveShift_{date:yyyyMMdd}_{dutyShiftId.ToString().ToLower()}";
            await _hubContext.Clients.Group(roomName).SendAsync("ReceiveShiftStructureChanged");
        }
    }
}
