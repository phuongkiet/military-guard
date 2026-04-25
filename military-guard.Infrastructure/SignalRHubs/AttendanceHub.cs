using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace military_guard.Infrastructure.SignalRHubs
{
    [Authorize]
    public class AttendanceHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var shiftId = httpContext?.Request.Query["shiftId"].ToString().ToLower();

            if (!string.IsNullOrEmpty(shiftId))
            {
                var today = DateTime.Now.ToString("yyyyMMdd");
                string roomName = $"LiveShift_{today}_{shiftId}";

                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var httpContext = Context.GetHttpContext();
            var shiftId = httpContext?.Request.Query["shiftId"].ToString().ToLower();

            if (!string.IsNullOrEmpty(shiftId))
            {
                var today = DateTime.Now.ToString("yyyyMMdd");
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"LiveShift_{today}_{shiftId}");
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinShiftRoom(string shiftAssignmentId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"ShiftRoom_{shiftAssignmentId}");
        }

        public async Task LeaveShiftRoom(string shiftAssignmentId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"ShiftRoom_{shiftAssignmentId}");
        }

        public async Task JoinCommanderGroup()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Commanders");
        }
    }
}
