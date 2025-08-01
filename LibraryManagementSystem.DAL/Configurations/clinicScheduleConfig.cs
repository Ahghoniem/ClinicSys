using LibraryManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.Configurations
{
    public class clinicScheduleConfig : IEntityTypeConfiguration<Clinic_schedule>
    {
        public void Configure(EntityTypeBuilder<Clinic_schedule> builder)
        {
            builder.HasOne(b => b.Department)
                    .WithMany()
                    .HasForeignKey(b => b.DepID)
                    .OnDelete(DeleteBehavior.Cascade);

            

            builder.HasOne(b => b.Doctor)
                    .WithMany()
                    .HasForeignKey(b => b.DID)
                    .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
