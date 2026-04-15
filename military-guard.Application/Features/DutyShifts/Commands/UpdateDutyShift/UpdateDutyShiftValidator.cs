using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.DutyShifts.Commands.UpdateDutyShift
{
    public class UpdateDutyShiftValidator : AbstractValidator<UpdateDutyShiftCommand>
    {
        public UpdateDutyShiftValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.StartTime).NotEmpty();
            RuleFor(x => x.EndTime).NotEmpty();
            RuleFor(x => x.ShiftOrder).GreaterThan(0);
        }
    }
}
