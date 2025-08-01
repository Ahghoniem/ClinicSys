using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.DTOs.UpdateUserDataDTOs
{

    public class UpdateBirthDateDto
    {
        [Required(ErrorMessage = "Id is required.")]
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "BirthDate is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateOnly BirthDate { get; set; }
    }
}
