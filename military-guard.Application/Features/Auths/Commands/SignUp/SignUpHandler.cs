using MediatR;
using military_guard.Application.Interfaces;
using military_guard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Application.Features.Auths.Commands.SignUp
{
    public class SignUpHandler : IRequestHandler<SignUpCommand, Guid>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SignUpHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            // 1. Trạm kiểm soát: Kiểm tra Username đã tồn tại chưa
            bool isUnique = await _accountRepository.IsUsernameUniqueAsync(request.Username);
            if (!isUnique)
            {
                throw new Exception("Tên đăng nhập này đã tồn tại trong hệ thống.");
            }

            // 2. Hash mật khẩu (Mã hóa một chiều)
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // 3. Tạo Entity Account
            var account = new Account
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                PasswordHash = hashedPassword,
                Role = request.Role,
                MilitiaId = request.MilitiaId
            };

            // 4. Lưu vào Database
            await _accountRepository.AddAsync(account);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return account.Id;
        }
    }
}
