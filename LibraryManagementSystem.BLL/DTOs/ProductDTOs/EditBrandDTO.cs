using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.DTOs.ProductDTOs
{
    public class EditBrandDTO
    {
        [Required(ErrorMessage = "The Department Id field is required.")]
        public int BrandId { get; set; }
        [Required(ErrorMessage = "The Department Name field is required.")]
        public string BrandName { get; set; }
    }
}
