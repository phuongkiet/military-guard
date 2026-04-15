using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.DutyShifts.Commands.DeleteDutyShift
{
    public record DeleteDutyShiftCommand(Guid Id) : IRequest<Unit>;
}
