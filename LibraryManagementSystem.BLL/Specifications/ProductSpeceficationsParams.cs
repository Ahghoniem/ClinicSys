using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.Specifications
{
    public class ProductSpeceficationsParams
    {
        private int pageSize = 5;
        private string? search;
        private const int maxSize = 10;

        public string? Sort { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public string? Search { get => search; set => search = value?.ToUpper(); }
        public int PageSize { get => pageSize; set => pageSize = value > maxSize ? maxSize : value; }
        public int PageIndex { get; set; } = 1;
    }
}
