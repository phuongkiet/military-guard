using MediatR;
using military_guard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.Auths.Commands.SignUp
{
    public record SignUpCommand(
    string Username,
    string Password,
    SystemRole Role,
    Guid? MilitiaId // Nếu tạo tk cho Admin thì để null, cho lính thì truyền ID hồ sơ vào
) : IRequest<Guid>;
}
