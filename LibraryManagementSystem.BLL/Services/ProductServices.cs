using AutoMapper;
using LibraryManagementSystem.BLL.DTOs.ProductDTOs;
using LibraryManagementSystem.BLL.Helpers;
using LibraryManagementSystem.BLL.ServicesContracts;
using LibraryManagementSystem.BLL.Specifications;
using LibraryManagementSystem.DAL.Entities.Products;
using LibraryManagementSystem.DAL.UOW;
using LibraryManagementSystem.BLL.Exceptions;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using LinkDev.Talabat.Core.Domain.Specifications.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BLL.Services
{
    public class ProductServices(IUnitOfWork unitOfWork, IMapper mapper) : IProductServices
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<int> AddBrandAsync(AddBrandDTO addBrandDTO)
        {
            if (addBrandDTO == null)
                throw new ArgumentNullException(nameof(addBrandDTO));

            var brand = new ProductBrand
            {
                Name = addBrandDTO.BrandName,

            };

            await unitOfWork.GetRepository<ProductBrand>().AddAsync(brand);

            int affectedRows = await unitOfWork.SaveAsync();

            return affectedRows;
        }


        public async Task DeleteBrandByIdAsync(int id)
        {
            var brand = await unitOfWork.GetRepository<ProductBrand>().GetByIdAsync(id);

            if (brand == null)
                throw new NotFoundExceptions("Brand", id);

            bool isUsed = await unitOfWork.GetRepository<Product>()
                                          .AnyAsync(p => p.CategoryId == id);

            if (isUsed)
                throw new BadRequestExceptions("Cannot delete brand because it is associated with existing products.");

            bool deleted = unitOfWork.GetRepository<ProductBrand>().Delete(brand);

            if (!deleted)
                throw new BadRequestExceptions("Failed to delete the brand. Please try again.");

            int affectedRows = await unitOfWork.SaveAsync();

            if (affectedRows == 0)
                throw new BadRequestExceptions("No records were affected. Brand might not have been deleted.");
        }


        public async Task<IEnumerable<BrandDTO>> GetBrandsAsync()
        {


            var Brands = await unitOfWork.GetRepository<ProductBrand>().GetAllAsync();

            var returnBrands = mapper.Map<IEnumerable<BrandDTO>>(Brands);
            return returnBrands;
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync()
        {
            var Categoriess = await unitOfWork.GetRepository<ProductCategory>().GetAllAsync();

            var returnCategories = mapper.Map<IEnumerable<CategoryDTO>>(Categoriess);
            return returnCategories;
        }

        public async Task<ProductToReturnDTO> GetProductById(int id)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(id);

            var product = await unitOfWork.GetRepository<Product>().GetByIdWithSpecAsync(spec);

            if (product is null)
                throw new NotFoundExceptions(nameof(product), id);

            var ProductToReturn = mapper.Map<ProductToReturnDTO>(product);

            return ProductToReturn;
        }

        public async Task<Pagination<ProductToReturnDTO>> GetProductsAsync(ProductSpeceficationsParams parameters)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(parameters.Sort, parameters.BrandId, parameters.CategoryId, parameters.PageSize, parameters.PageIndex, parameters.Search);

            var products = await unitOfWork.GetRepository<Product>().GetAllWithSpecAsync(spec);

            var ProductsToReturn = mapper.Map<IEnumerable<ProductToReturnDTO>>(products);

            var countSpec = new ProductWithFilterationForCountSpecifications(parameters.BrandId, parameters.CategoryId, parameters.Search);
            var count = await unitOfWork.GetRepository<Product>().GetCountAsync(countSpec);

            return new Pagination<ProductToReturnDTO>(parameters.PageIndex, parameters.PageSize, ProductsToReturn, count);
        }

        public async Task<ProductBrand> UpdateBrandAsync(EditBrandDTO editBrandDTO)
        {

            if (editBrandDTO == null)
                throw new ArgumentNullException(nameof(editBrandDTO));


            if (editBrandDTO.BrandId <= 0)
                throw new ArgumentException("Invalid Brand ID.");


            var brand = await unitOfWork.GetRepository<ProductBrand>().GetByIdAsync(editBrandDTO.BrandId);


            if (brand == null)
                throw new NotFoundExceptions($"Brand with ID {editBrandDTO.BrandId} not found.", editBrandDTO.BrandId);


            brand.Name = editBrandDTO.BrandName;

            bool updated = unitOfWork.GetRepository<ProductBrand>().Update(brand);

            if (!updated)
                throw new InvalidOperationException("Failed to update the brand.");

            int affectedRows = await unitOfWork.SaveAsync();

            if (affectedRows == 0)
                throw new InvalidOperationException("No records were affected. Brand update might have failed.");

            return brand;
        }

    }
}
