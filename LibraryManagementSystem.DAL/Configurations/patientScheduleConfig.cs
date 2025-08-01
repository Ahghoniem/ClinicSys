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
    public class patientScheduleConfig : IEntityTypeConfiguration<Patient_schedule>
    {
        public void Configure(EntityTypeBuilder<Patient_schedule> builder)
        {
            builder.HasOne(b => b.Member)
                    .WithMany()
                    .HasForeignKey(b => b.PID)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.Doctor)
                  .WithMany()
                  .HasForeignKey(b => b.DID)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.schedule)
                  .WithMany()
                  .HasForeignKey(b => b.SId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
