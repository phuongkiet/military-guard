using MediatR;
using military_guard.Application.Common.Models;
using military_guard.Application.Features.Accounts.DTOs;
using military_guard.Application.Features.GuardPosts.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.Accounts.Queries.GetAllAccounts
{
    public record GetAllAccountsQuery : IRequest<PaginatedList<AccountResponse>>
    {
        public string? SearchTerm { get; init; }
        public bool? IsActive { get; init; }
        public int PageIndex { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}
