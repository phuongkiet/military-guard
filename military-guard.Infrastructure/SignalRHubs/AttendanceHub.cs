using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace military_guard.Infrastructure.SignalRHubs
{
    [Authorize]
    public class AttendanceHub : Hub
    {
        public async Task JoinCommanderGroup()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Commanders");
        }
    }
}
