using LibraryManagementSystem.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.DTOs.AuthDTOs
{
    public class ApplicationUserDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int UserType { get; set; }
        public string? ImageUrl { get; set; }
        public string? Address { get; set; }
        public Gender? Gender { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string BloodType { get; set; }

    }

}
