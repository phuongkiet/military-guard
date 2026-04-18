using MediatR;
using military_guard.Application.Common.Models;
using military_guard.Application.Features.Accounts.DTOs;
using military_guard.Application.Features.GuardPosts.DTOs;
using military_guard.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.Accounts.Queries.GetAllAccounts
{
    public class GetAllAccountsHandler : IRequestHandler<GetAllAccountsQuery, PaginatedList<AccountResponse>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;
        public GetAllAccountsHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedList<AccountResponse>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
        {
            var pagedEntities = await _accountRepository.GetPagedAccountsAsync(
            request.SearchTerm,
            request.IsActive,
            request.PageIndex,
            request.PageSize);

            var dtos = pagedEntities.Items.Select(g => new AccountResponse(
                g.Id,
                g.Username,
                g.Role.ToString(),
                g.MilitiaId,
                g.IsDeleted,
                g.IsBanned
            )).ToList();

            return new PaginatedList<AccountResponse>(
                dtos,
                pagedEntities.TotalCount,
                request.PageIndex,
                request.PageSize);
        }
    }
}
