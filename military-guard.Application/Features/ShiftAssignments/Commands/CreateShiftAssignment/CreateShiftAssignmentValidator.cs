using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.ShiftAssignments.Commands.CreateShiftAssignment
{
    public class CreateShiftAssignmentValidator : AbstractValidator<CreateShiftAssignmentCommand>
    {
        public CreateShiftAssignmentValidator()
        {
            RuleFor(x => x.MilitiaId).NotEmpty();
            RuleFor(x => x.GuardPostId).NotEmpty();
            RuleFor(x => x.DutyShiftId).NotEmpty();

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Ngày trực không được để trống.")
                .Must(date => date >= DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("Không thể xếp lịch trực cho thời gian trong quá khứ.");
        }
    }
}
