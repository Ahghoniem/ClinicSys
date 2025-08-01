using LibraryManagementSystem.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.Entities.IdentityEntities
{
    public class Patient : ApplicationUser
    {
        public string? ImageUrl { get; set; }
        public string? Address { get; set; }

        public Gender? Gender { get; set; }

        public DateOnly ?BirthDate { get; set; }
        public string? BloodType { get; set; }
    }
}
