using MediatR;
using military_guard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.Accounts.Commands.UpdateAccount
{
    public record UpdateAccountCommand(
    Guid Id,
    SystemRole Role,
    Guid? MilitiaId
) : IRequest<bool>;
}
