using MediatR;
using military_guard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.LeaveRequests.Commands.ProcessLeaveRequest
{
    public class ProcessLeaveRequestCommand : IRequest<Unit>
    {
        // ID của đơn cần xử lý
        public Guid LeaveRequestId { get; set; }

        // Trạng thái mới: Approved hoặc Rejected
        public LeaveStatus NewStatus { get; set; }
    }
}
