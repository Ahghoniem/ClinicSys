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
    internal class AdminConfig : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.HasOne(b => b.Department)
                    .WithMany()
                    .HasForeignKey(b => b.DepID)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
