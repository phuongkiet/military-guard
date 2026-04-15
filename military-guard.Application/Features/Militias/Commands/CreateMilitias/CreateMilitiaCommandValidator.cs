using FluentValidation;
using military_guard.Domain.Enums;

namespace military_guard.Application.Features.Militias.Commands.CreateMilitias;

public class CreateMilitiaCommandValidator : AbstractValidator<CreateMilitiaCommand>
{
    public CreateMilitiaCommandValidator()
    {
        RuleFor(v => v.FullName)
            .NotEmpty().WithMessage("Tên dân quân không được để trống.")
            .MaximumLength(200).WithMessage("Tên không vượt quá 200 ký tự.");

        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("Email không được để trống.")
            .EmailAddress().WithMessage("Email không đúng định dạng.");

        // Kiểm tra xem dữ liệu truyền vào có nằm trong Enum không
        RuleFor(v => v.Type)
            .IsInEnum().WithMessage("Loại hình dân quân không hợp lệ.");

        RuleFor(v => v.Rank)
            .IsInEnum().WithMessage("Cấp bậc không hợp lệ.");

        RuleFor(v => v.JoinDate)
            .NotEmpty().WithMessage("Ngày tham gia không được để trống.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Ngày tham gia không được vượt quá ngày hiện tại.");
    }
}