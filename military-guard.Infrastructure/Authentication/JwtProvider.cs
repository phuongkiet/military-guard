using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using military_guard.Application.Interfaces;
using military_guard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace military_guard.Infrastructure.Authentication
{
    public class JwtProvider : IJwtProvider
    {
        private readonly IConfiguration _configuration;

        public JwtProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Account account)
        {
            // 1. Lấy thông tin từ appsettings.json
            var secretKey = _configuration["Jwt:Secret"]!;
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            // 2. Tạo danh sách các "mảnh thông tin" (Claims) đính kèm vào Token
            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, account.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, account.Username),
            new(ClaimTypes.Role, account.Role.ToString()), // Lưu quyền (Admin/Commander/Militia)
        };

            // Nếu là Dân quân thì đính kèm thêm MilitiaId để Frontend/Backend dễ dùng
            if (account.MilitiaId.HasValue)
            {
                claims.Add(new Claim("militia_id", account.MilitiaId.ToString()!));
            }

            // 3. Ký tên và tạo Token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"]!)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
