using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.DutyShifts.Commands.CreateDutyShift
{
    public class CreateDutyShiftValidator : AbstractValidator<CreateDutyShiftCommand>
    {
        public CreateDutyShiftValidator()
        {
            RuleFor(x => x.StartTime).NotNull().WithMessage("Giờ bắt đầu không được để trống.");
            RuleFor(x => x.EndTime).NotNull().WithMessage("Giờ kết thúc không được để trống.");

            RuleFor(x => x.ShiftOrder)
                .GreaterThan(0).WithMessage("Thứ tự ca phải là số dương (1, 2, 3...).");
        }
    }
}
