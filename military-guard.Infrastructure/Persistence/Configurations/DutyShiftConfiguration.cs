using military_guard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace military_guard.Infrastructure.Persistence.Configurations
{
    public class DutyShiftConfiguration : IEntityTypeConfiguration<DutyShift>
    {
        public void Configure(EntityTypeBuilder<DutyShift> builder)
        {
            builder.ToTable("DutyShifts");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.StartTime).IsRequired();
            builder.Property(e => e.EndTime).IsRequired();
            builder.Property(e => e.ShiftOrder).IsRequired();

            builder.HasMany(e => e.ShiftAssignments)
                   .WithOne(sa => sa.DutyShift)
                   .HasForeignKey(sa => sa.DutyShiftId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
