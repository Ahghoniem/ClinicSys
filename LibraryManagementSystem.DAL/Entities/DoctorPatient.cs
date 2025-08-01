using LibraryManagementSystem.DAL.Entities.IdentityEntities;
using LibraryManagementSystem.DAL.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.Entities
{
    public class DoctorPatient
    {
        public Clinic_schedule schedule { get; set; }
        public Patient user { get; set; }

        public Department Department { get; set; }
    }
}
