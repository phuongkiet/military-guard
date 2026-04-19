namespace military_guard.API.Extensions
{
    public static class CorsConfiguration
    {
        public const string AllowFrontendPolicy = "AllowFrontend";

        // Hàm mở rộng cho IServiceCollection
        public static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: AllowFrontendPolicy,
                    policy =>
                    {
                        policy.WithOrigins(
                                "http://localhost:5173",  
                                "https://localhost:5173"
                            )
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });

            return services;
        }
    }
}
