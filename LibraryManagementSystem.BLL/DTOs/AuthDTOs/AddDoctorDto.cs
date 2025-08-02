using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.DTOs.AuthDTOs
{
    public class AddDoctorDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public int DepID { get; set; }
        public string FacultyGraduation { get; set; }

        [Required]
        public IFormFile ImageUrl { get; set; } = null!;
        public string PhoneNumber { get; set; }

    }
}
