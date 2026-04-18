using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.Accounts.Commands.BanAccount
{
    public record BanAccountCommand(Guid AccountId) : IRequest<bool>;
}
