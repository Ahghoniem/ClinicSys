using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.DTOs
{
    public class UpdateClinicDoctorDTO
    {
        public int[] SId{ get; set; }
        public string[] day { get; set; }
        public int[] shift { get; set; }
    }
}
