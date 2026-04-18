using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.Accounts.Commands.UnbanAccount
{
    public record UnbanAccountCommand(Guid AccountId) : IRequest<bool>;
}
