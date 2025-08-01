using LibraryManagementSystem.DAL.Entities.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.Entities
{
    public class usersAndSchedule
    {
        public Patient_schedule schedule { get; set; }
        public ApplicationUser patient { get; set; }

        public ApplicationUser doctor { get; set; }
    }
}
