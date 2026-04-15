using military_guard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace military_guard.Infrastructure.Persistence.Configurations;

public class GuardPostConfiguration : IEntityTypeConfiguration<GuardPost>
{
    public void Configure(EntityTypeBuilder<GuardPost> builder)
    {
        builder.ToTable("GuardPosts");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Location).HasMaxLength(500);

        // Ràng buộc số người tối thiểu/tối đa
        builder.Property(e => e.MinPersonnel).HasDefaultValue(2);
        builder.Property(e => e.MaxPersonnel).HasDefaultValue(4);

        builder.HasMany(e => e.ShiftAssignments)
               .WithOne(sa => sa.GuardPost)
               .HasForeignKey(sa => sa.GuardPostId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}