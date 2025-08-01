using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.DTOs.AuthDTOs
{
    public class DoctorDto
    {
        public string Id { get; set; } 
        public string FullName { get; set; }
        public string Email { get; set; } 
        //public int DepID { get; set; }
        public string FacultyGraduation { get; set; }
        public string? ImageUrl { get; set; }
        public int DepartmentId { get; set; }
        public string PhoneNumber { get; set; }


    }
}
