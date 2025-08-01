using LibraryManagementSystem.DAL.Entities.Products;
using LibraryManagementSystem.DAL.Specifications;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Specifications.Products
{
    public class ProductWithFilterationForCountSpecifications : BaseSpecifications<Product>
    {
        public ProductWithFilterationForCountSpecifications(int? brandId, int? categoryId,string? search) : base(
             p =>
                                  (string.IsNullOrEmpty(search) || p.NormalizedName.Contains(search))
                        &&

                     (!brandId.HasValue || p.BrandId == brandId.Value)
                        &&
                    (!categoryId.HasValue || p.CategoryId == categoryId.Value)

            )
        {
        }
    }
}
