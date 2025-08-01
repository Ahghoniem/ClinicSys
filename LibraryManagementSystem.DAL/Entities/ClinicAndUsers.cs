using LibraryManagementSystem.DAL.Entities.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.Entities
{
    public class ClinicAndUsers
    {
        public Clinic_schedule schedule { get; set; }
        public ApplicationUser user { get; set; }

        public Department Department { get; set; }

    }
}
