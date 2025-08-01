using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.DTOs.UpdateUserDataDTOs
{
    public class UpdateAddressDto
    {
        [Required(ErrorMessage = "Id is required.")]
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required.")]
        [MinLength(5, ErrorMessage = "Address must be at least 5 characters.")]
        public string Address { get; set; } = string.Empty;
    }
}
