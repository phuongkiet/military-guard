using military_guard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace military_guard.Infrastructure.Persistence.Configurations;

public class ShiftAssignmentConfiguration : IEntityTypeConfiguration<ShiftAssignment>
{
    public void Configure(EntityTypeBuilder<ShiftAssignment> builder)
    {
        builder.ToTable("ShiftAssignments");
        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.Militia)
               .WithMany(m => m.ShiftAssignments)
               .HasForeignKey(e => e.MilitiaId)
               .OnDelete(DeleteBehavior.Restrict);

        // 1 Người trong 1 Ngày và 1 Khung giờ chỉ được tồn tại ĐÚNG 1 dòng.
        // Cấm tuyệt đối trường hợp 1 Dân quân vừa trực Chốt A vừa trực Chốt B cùng một lúc.
        builder.HasIndex(e => new { e.MilitiaId, e.Date, e.DutyShiftId }).IsUnique();
    }
}