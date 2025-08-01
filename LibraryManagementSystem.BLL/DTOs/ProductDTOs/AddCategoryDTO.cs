using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.DTOs.ProductDTOs
{
    public class AddCategoryDTO
    {
        [Required(ErrorMessage = "The Department Name field is required.")]
        public string? CategoryName { get; set; }
    }
}
