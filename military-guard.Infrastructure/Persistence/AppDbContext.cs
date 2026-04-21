using military_guard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Militia> Militias => Set<Militia>();
        public DbSet<DutyShift> DutyShifts => Set<DutyShift>();
        public DbSet<ShiftAssignment> ShiftAssignments => Set<ShiftAssignment>();
        public DbSet<GuardPost> GuardPosts => Set<GuardPost>();
        public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();
        public DbSet<Attendance> Attendances => Set<Attendance>();
        public DbSet<HolidayEvent> HolidayEvents => Set<HolidayEvent>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
