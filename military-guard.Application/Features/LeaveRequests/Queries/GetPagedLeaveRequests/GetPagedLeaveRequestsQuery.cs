using MediatR;
using military_guard.Application.Common.Models;
using military_guard.Application.Features.LeaveRequests.DTOs;
using military_guard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.LeaveRequests.Queries.GetPagedLeaveRequests
{
    public record GetPagedLeaveRequestsQuery : IRequest<PaginatedList<LeaveRequestResponse>>
    {
        // Bộ lọc: Lọc theo dân quân hoặc theo trạng thái đơn (Chờ duyệt, Đã duyệt...)
        public Guid? MilitiaId { get; init; }
        public LeaveStatus? Status { get; init; }

        // Phân trang
        public int PageIndex { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}
