using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.DutyShifts.Commands.UpdateDutyShift
{
    public class UpdateDutyShiftCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int ShiftOrder { get; set; }
    }
}
