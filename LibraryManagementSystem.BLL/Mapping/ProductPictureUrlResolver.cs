using AutoMapper;
using LibraryManagementSystem.BLL.DTOs;
using LibraryManagementSystem.BLL.DTOs.ProductDTOs;
using LibraryManagementSystem.DAL.Entities;
using LibraryManagementSystem.DAL.Entities.Products;
using Microsoft.Extensions.Configuration;

namespace LinkDev.Talabat.Core.Application.Mapping
{
    internal class ProductPictureUrlResolver(IConfiguration configuration) : IValueResolver<Product, ProductToReturnDTO, string?>
    {

        public string? Resolve(Product source, ProductToReturnDTO destination, string? destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{configuration["Urls:ApiBaseURl"]}/{source.PictureUrl}";
            }

            return string.Empty;
        }
    }

    internal class DepartmentPictureUrlResolver(IConfiguration configuration)
    : IValueResolver<Department, DepartmentDTO, string?>
    {
        public string? Resolve(Department source, DepartmentDTO destination, string? destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ImageUrl))
            {
                return $"{configuration["Urls:ApiBaseURl"]}/{source.ImageUrl}";
            }

            return string.Empty;
        }
    }


}
