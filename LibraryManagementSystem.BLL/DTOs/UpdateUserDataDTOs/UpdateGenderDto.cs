using LibraryManagementSystem.DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.DTOs.UpdateUserDataDTOs
{
    public class UpdateGenderDto
    {
        [Required(ErrorMessage = "Id is required.")]
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gender is required.")]
        [EnumDataType(typeof(Gender), ErrorMessage = "Invalid gender value.")]
        public Gender Gender { get; set; }
    }
}
