using MediatR;
using military_guard.Application.Features.Accounts.DTOs;
using military_guard.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.Accounts.Queries.GetAccountById
{
    public class GetAccountByIdHandler : IRequestHandler<GetAccountByIdQuery, AccountResponse?>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAccountByIdHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<AccountResponse?> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetByIdAsync(request.Id);

            if (account == null) return null;

            return new AccountResponse(
                Id: account.Id,
                Username: account.Username,
                Role: account.Role.ToString(),
                MilitiaId: account.MilitiaId,
                IsDeleted: account.IsDeleted,
                IsBanned: account.IsBanned
            );
        }
    }
}
