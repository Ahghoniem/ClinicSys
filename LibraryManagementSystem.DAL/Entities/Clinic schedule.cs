using LibraryManagementSystem.DAL.Entities.IdentityEntities;
using LibraryManagementSystem.DAL.Entities.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.Entities
{
    public class Clinic_schedule
    {
        public int ID { get; set; }


        public string? DID { get; set; }

        public Doctor? Doctor { get; set; }

        [Required]
        public int DepID { get; set; } // foreign key

        public virtual Department? Department { get; set; }

        [Required]

        public int shift { get; set; }

        [Required]

        public string day { get; set; }

        [Required]

        public DateTime date { get; set; }

        [Required]

        public int count { get; set; }

        [Required]

        public bool status { get; set; }


    }
}
