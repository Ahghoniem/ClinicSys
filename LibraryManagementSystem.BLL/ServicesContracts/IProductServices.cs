using LibraryManagementSystem.BLL.DTOs.ProductDTOs;
using LibraryManagementSystem.BLL.Helpers;
using LibraryManagementSystem.BLL.Specifications;
using LibraryManagementSystem.DAL.Entities.Products;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.ServicesContracts
{
    public interface IProductServices
    {
        Task<Pagination<ProductToReturnDTO>> GetProductsAsync(ProductSpeceficationsParams parameters);

        Task<ProductToReturnDTO> GetProductById(int id);
        Task<IEnumerable<BrandDTO>> GetBrandsAsync();

        //Task<BrandDTO> GetBrandById(int id);
        Task<IEnumerable<CategoryDTO>> GetCategoriesAsync();

        //Task<CategoryDTO> GetCategoryById(int id);

        Task<int> AddBrandAsync(AddBrandDTO addBrandDTO);
        public  Task<ProductBrand> UpdateBrandAsync(EditBrandDTO editBrandDTO);
        Task DeleteBrandByIdAsync(int id);

    }
}
