using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.DutyShifts.Commands.CreateDutyShift
{
    public record CreateDutyShiftCommand(TimeOnly? StartTime, TimeOnly? EndTime, int ShiftOrder) : IRequest<Guid>;
}
