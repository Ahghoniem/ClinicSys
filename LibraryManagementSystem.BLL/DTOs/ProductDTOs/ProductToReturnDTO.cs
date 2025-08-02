using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.DTOs.ProductDTOs
{
    public class    ProductToReturnDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public string? PictureUrl { get; set; }

        public decimal Price { get; set; }

        public int? BrandId { get; set; } // Foreign Key For Product Brand Entity
        public string? Brand { get; set; }
        public int? CategoryId { get; set; } // Foreign Key For Product Category Entity   
        public string? Category { get; set; }

    }
}
