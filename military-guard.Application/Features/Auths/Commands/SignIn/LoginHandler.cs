using MediatR;
using military_guard.Application.Features.Auths.DTOs;
using military_guard.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;

namespace military_guard.Application.Features.Auths.Commands.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IJwtProvider _jwtProvider;

        public LoginHandler(IAccountRepository accountRepository, IJwtProvider jwtProvider)
        {
            _accountRepository = accountRepository;
            _jwtProvider = jwtProvider;
        }

        public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // 1. Tìm tài khoản theo Username
            var account = await _accountRepository.GetByUsernameAsync(request.Username);

            if (account == null)
            {
                throw new InvalidCredentialException("Tài khoản hoặc mật khẩu không chính xác.");
            }

            // 2. Xác thực mật khẩu bằng BCrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, account.PasswordHash);

            if (!isPasswordValid)
            {
                throw new InvalidCredentialException("Tài khoản hoặc mật khẩu không chính xác.");
            }

            // 3. Tạo JWT Token
            string token = _jwtProvider.GenerateToken(account);

            // 4. Trả về DTO cho Frontend
            return new AuthResponse(
                Token: token,
                Username: account.Username,
                Role: account.Role.ToString(),
                MilitiaId: account.MilitiaId
            );
        }
    }
}
