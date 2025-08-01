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
    internal class PatientConfig : IEntityTypeConfiguration<Patient>
    {

        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            

            //builder.Property(b => b.Address).IsRequired();
            //builder.Property(b => b.BirthDate).IsRequired();
        }
    }
}
