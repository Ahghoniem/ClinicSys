using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.DTOs.UpdateUserDataDTOs
{
    public class UpdateImageUrlDto
    {
        [Required(ErrorMessage = "Id is required.")]
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "ImageUrl is required.")]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
