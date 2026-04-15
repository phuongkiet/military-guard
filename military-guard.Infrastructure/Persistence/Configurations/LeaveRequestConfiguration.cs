using military_guard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Infrastructure.Persistence.Configurations
{
    public class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
    {
        public void Configure(EntityTypeBuilder<LeaveRequest> builder)
        {
            builder.ToTable("LeaveRequests");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Reason).HasMaxLength(500);

            builder.HasOne(e => e.Militia)
                   .WithMany(e => e.LeaveRequests)
                   .HasForeignKey(e => e.MilitiaId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình người duyệt
            builder.HasOne(e => e.Approver)
                   .WithMany()
                   .HasForeignKey(e => e.ApproverId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
