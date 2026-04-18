using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.Accounts.Commands.DeleteAccount
{
    public record DeleteAccountCommand(Guid Id) : IRequest<bool>;
}
