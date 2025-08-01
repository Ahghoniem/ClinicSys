using LibraryManagementSystem.DAL.Entities.IdentityEntities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users");

            // Configure only the desired properties
            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.UserType)
                .IsRequired();

            builder.Ignore(u => u.AccessFailedCount);
            builder.Ignore(u => u.ConcurrencyStamp);
            builder.Ignore(u => u.EmailConfirmed);
            builder.Ignore(u => u.LockoutEnabled);
            builder.Ignore(u => u.LockoutEnd);
            //builder.Ignore(u => u.NormalizedEmail);
            //builder.Ignore(u => u.NormalizedUserName);
            builder.Ignore(u => u.PhoneNumberConfirmed);
            builder.Ignore(u => u.SecurityStamp);
            builder.Ignore(u => u.TwoFactorEnabled);
            //builder.Ignore(u => u.UserName);
        }
    }
}
