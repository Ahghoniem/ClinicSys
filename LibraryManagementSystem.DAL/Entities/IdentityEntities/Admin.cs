using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.Entities.IdentityEntities
{
    public class Admin :ApplicationUser
    {
        public int DepID { get; set; }
        public Department Department { get; set; }
    }
}
