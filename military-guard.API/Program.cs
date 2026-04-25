using Hangfire;
using military_guard.API.ExceptionHandlers;
using military_guard.API.Extensions;
using military_guard.Application;
using military_guard.Application.Interfaces;
using military_guard.Infrastructure;
using military_guard.Infrastructure.SignalRHubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddCustomCors();

var app = builder.Build();

app.UseHangfireDashboard("/hangfire");

RecurringJob.AddOrUpdate<IShiftSchedulingService>(
    "auto-generate-weekly-schedule", 
    service => service.GenerateWeeklyScheduleAsync(),
    "0 1 * * 0",
    new RecurringJobOptions { TimeZone = TimeZoneInfo.Local }
);

RecurringJob.AddOrUpdate<IShiftSchedulingService>(
    "auto-notify-36h",
    service => service.ScanAndNotifyUpcomingShiftsAsync(),
    "0 * * * *", 
    new RecurringJobOptions { TimeZone = TimeZoneInfo.Local }
);

RecurringJob.AddOrUpdate<IAttendanceBackgroundService>(
    "auto-close-attendance-missing",
    service => service.AutoCloseAttendanceAsync(),
    "*/5 * * * *",
    new RecurringJobOptions { TimeZone = TimeZoneInfo.Local }
);

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(CorsConfiguration.AllowFrontendPolicy);

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapHub<AttendanceHub>("/api/hubs/attendance");

app.MapControllers();

app.Run();
