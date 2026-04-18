using MediatR;
using military_guard.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.Accounts.Commands.UnbanAccount
{
    public class UnbanAccountHandler : IRequestHandler<UnbanAccountCommand, bool>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UnbanAccountHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UnbanAccountCommand request, CancellationToken cancellationToken)
        {
            await _accountRepository.UnbanUser(request.AccountId);

            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
