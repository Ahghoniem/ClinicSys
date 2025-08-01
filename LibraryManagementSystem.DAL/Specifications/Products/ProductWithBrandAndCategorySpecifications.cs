using LibraryManagementSystem.DAL.Entities.Products;
using LibraryManagementSystem.DAL.Specifications;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Specifications.Products
{
    public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product>
    {
        public ProductWithBrandAndCategorySpecifications(string? sort, int? brandId, int? categoryId , int pageSize , int pageIndex, string? search) 
            : base(
                  p=>
                     (string.IsNullOrEmpty(search)||p.NormalizedName.Contains(search)) 
                        &&
                     ( !brandId.HasValue || p.BrandId == brandId.Value)
                        &&
                    ( !categoryId.HasValue||p.CategoryId == categoryId.Value)
                  
                  )
        {
            Includes.Add(p => p.Brand!);
            Includes.Add(p => p.Category!);

            AddOrderBy(p => p.Id);
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "nameDesc":
                        AddOrderByDesc(p => p.Name);
                        break;
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }

            ApplyPagination((pageIndex - 1) * pageSize, pageSize);
        }

        public ProductWithBrandAndCategorySpecifications(int id) : base(id)
        {
            Includes.Add(p => p.Brand!);
            Includes.Add(p => p.Category!);
        }
    }
}
