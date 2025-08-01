using LibraryManagementSystem.DAL.Entities.IdentityEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.Configurations
{
    class doctorConfig : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasOne(b => b.Department)
                    .WithMany()
                    .HasForeignKey(b => b.DepID)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.Property(b => b.GraduationFaculty).IsRequired();
        }
    }
}
