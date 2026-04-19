using MediatR;
using military_guard.Application.Features.Attendances.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.Attendances.Commands.CheckIn
{
    public record CheckInCommand(
    Guid MilitiaId,
    Guid ShiftId
) : IRequest<AttendanceResponse>;
}
