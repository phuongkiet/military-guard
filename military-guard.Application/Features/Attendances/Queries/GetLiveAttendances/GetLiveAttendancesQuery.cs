using MediatR;
using military_guard.Application.Features.Attendances.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.Attendances.Queries.GetLiveAttendances
{
    public class GetLiveAttendancesQuery : IRequest<List<LiveAttendanceDto>>
    {
        public Guid ShiftId { get; set; }
        public DateOnly? Date { get; set; }
    }
}
