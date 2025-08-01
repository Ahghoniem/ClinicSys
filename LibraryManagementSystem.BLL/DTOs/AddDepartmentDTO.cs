using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.DTOs
{
    public class AddDepartmentDTO
    {
        [Required(ErrorMessage = "Department name is required.")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Department Description is required.")]

        [StringLength(100, ErrorMessage = "Department Description can't be longer than 100 characters.")]

        public string Dec { get; set; } = string.Empty;
    }
}
