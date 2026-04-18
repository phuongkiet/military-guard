using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using military_guard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace military_guard.Infrastructure.Persistence.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Accounts");
            builder.HasKey(e => e.Id);

            builder.HasOne(a => a.Militia)
                .WithMany() 
                .HasForeignKey(a => a.MilitiaId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
