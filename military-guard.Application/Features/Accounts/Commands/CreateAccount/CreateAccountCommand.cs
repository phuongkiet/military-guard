using MediatR;
using military_guard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.Accounts.Commands.CreateAccount
{
    public record CreateAccountCommand(
    string Username,
    string Password,
    SystemRole Role,
    Guid? MilitiaId
) : IRequest<Guid>;
}
