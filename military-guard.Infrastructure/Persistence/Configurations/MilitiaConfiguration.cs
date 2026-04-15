using military_guard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace military_guard.Infrastructure.Persistence.Configurations;

public class MilitiaConfiguration : IEntityTypeConfiguration<Militia>
{
    public void Configure(EntityTypeBuilder<Militia> builder)
    {
        builder.ToTable("Militias");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.FullName).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Email).IsRequired().HasMaxLength(150);

        builder.HasQueryFilter(e => !e.IsDeleted);

        builder.HasIndex(e => e.Email).IsUnique();

        builder.HasOne(e => e.Manager)
               .WithMany()
               .HasForeignKey(e => e.ManagerId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}