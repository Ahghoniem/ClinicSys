using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.DTOs
{
    public class GetDoctorScheduleDTO
    {
        public int SId { get; set; }
        public String DId { get; set; }
        public string DoctorName { get; set; }
        public int DepId { get; set; }
        public string DepartmentName { get; set; }
        public string day { get; set; }
        public int shift { get; set; }
        public DateTime date { get; set; }
    }
}
