using military_guard.Application.Interfaces;
using military_guard.Domain.Entities;
using military_guard.Infrastructure.Persistence;
using military_guard.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddScoped<IMilitiaRepository, MilitiaRepository>();
            services.AddScoped<IGuardPostRepository, GuardPostRepository>();
            services.AddScoped<IDutyShiftRepository, DutyShiftRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
