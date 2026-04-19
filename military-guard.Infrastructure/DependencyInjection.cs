using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using military_guard.Application.Interfaces;
using military_guard.Domain.Entities;
using military_guard.Infrastructure.Authentication;
using military_guard.Infrastructure.Persistence;
using military_guard.Infrastructure.Repositories;
using military_guard.Infrastructure.Services;
using System.Text;

namespace military_guard.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!))
                };
            });

            services.AddSignalR();

            services.AddScoped<IMilitiaRepository, MilitiaRepository>();
            services.AddScoped<IGuardPostRepository, GuardPostRepository>();
            services.AddScoped<IDutyShiftRepository, DutyShiftRepository>();
            services.AddScoped<IShiftAssignmentRepository, ShiftAssignmentRepository>();
            services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISignalRService, SignalRService>();
            services.AddScoped<IAttendanceRepository, AttendanceRepository>();

            return services;
        }
    }
}
