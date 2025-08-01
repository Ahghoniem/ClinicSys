using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.DTOs.UpdateUserDataDTOs
{
    public class UpdateBloodTypeDto
    {
        [Required(ErrorMessage = "Id is required.")]
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "BloodType is required.")]
        public string BloodType { get; set; } = string.Empty;
    }
}
