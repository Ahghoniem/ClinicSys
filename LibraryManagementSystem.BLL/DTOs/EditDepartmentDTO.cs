using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.DTOs
{
    public class EditDepartmentDTO
    {
        [Required(ErrorMessage = "Id is required.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Department name is required.")]
        [StringLength(100, ErrorMessage = "Department name can't be longer than 100 characters.")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Department Description is required.")]
        [StringLength(100, ErrorMessage = "Department Description can't be longer than 100 characters.")]
        public string Desc { get; set; } = string.Empty;

        [Required(ErrorMessage = "ImageUrl is required.")]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
