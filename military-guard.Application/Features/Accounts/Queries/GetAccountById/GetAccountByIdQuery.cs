using MediatR;
using military_guard.Application.Features.Accounts.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.Accounts.Queries.GetAccountById
{
    public record GetAccountByIdQuery(Guid Id) : IRequest<AccountResponse?>;
}
