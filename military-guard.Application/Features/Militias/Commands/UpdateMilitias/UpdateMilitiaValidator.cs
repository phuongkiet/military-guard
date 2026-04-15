using FluentValidation;

namespace military_guard.Application.Features.Militias.Commands.UpdateMilitias;

public class UpdateMilitiaCommandValidator : AbstractValidator<UpdateMilitiaCommand>
{
    public UpdateMilitiaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Tên dân quân không được để trống.")
            .MaximumLength(200).WithMessage("Tên không vượt quá 200 ký tự.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email không được để trống.")
            .EmailAddress().WithMessage("Email không đúng định dạng.");

        RuleFor(x => x.Type).IsInEnum().WithMessage("Loại hình dân quân không hợp lệ.");
        RuleFor(x => x.Rank).IsInEnum().WithMessage("Cấp bậc không hợp lệ.");

        RuleFor(x => x.JoinDate)
            .NotEmpty().WithMessage("Ngày tham gia không được để trống.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Ngày tham gia không được vượt quá ngày hiện tại.");

        // Chặn Dân quân tự làm chỉ huy của chính mình
        RuleFor(x => x.ManagerId)
            .NotEqual(x => x.Id)
            .When(x => x.ManagerId.HasValue)
            .WithMessage("Dân quân không thể tự làm chỉ huy của chính mình.");
    }
}