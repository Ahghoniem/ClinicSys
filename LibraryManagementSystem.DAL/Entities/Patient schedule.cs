using LibraryManagementSystem.DAL.Entities.IdentityEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.Entities
{
    public class Patient_schedule 
    {
        public int ID { get; set; }

        [Required]
        public string PID { get; set; }

        public ApplicationUser Member { get; set; }

        public string? DID { get; set; }

        public Doctor? Doctor { get; set; }

        public int? SId { get; set; }

        public Clinic_schedule? schedule { get; set; }

        [Required]

        public int shift { get; set; }

        [Required]

        public string day { get; set; }

        [Required]

        public DateTime date { get; set; }


        public string PrescriptionMedicine { get; set; }

        [Required]

        public string status { get; set; }

    }
}
