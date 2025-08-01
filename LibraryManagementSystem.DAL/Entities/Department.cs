﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DAL.Entities
{
    public class Department : BaseEntity
    {
        [Required]
        public required string DepName { get; set; }
        [Required]
        public required string DepDescription { get; set; }

        public string? ImageUrl { get; set; }


    }
}
