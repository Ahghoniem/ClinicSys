using AutoMapper;
using LibraryManagementSystem.BLL.DTOs;
using LibraryManagementSystem.BLL.DTOs.AuthDTOs;
using LibraryManagementSystem.BLL.DTOs.ProductDTOs;
using LibraryManagementSystem.DAL.Entities;
using LibraryManagementSystem.DAL.Entities.IdentityEntities;
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


    internal class DoctorPictureUrlResolver(IConfiguration configuration)
    : IValueResolver<Doctor, DoctorDto, string?>
    {
        public string? Resolve(Doctor source, DoctorDto destination, string? destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ImageUrl))
            {
                return $"{configuration["Urls:ApiBaseURl"]}/{source.ImageUrl}";
            }

            return string.Empty;
        }
    }

    internal class AdminPictureUrlResolver(IConfiguration configuration)
: IValueResolver<Admin, AdminDto, string?>
    {
        public string? Resolve(Admin source, AdminDto destination, string? destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ImageUrl))
            {
                return $"{configuration["Urls:ApiBaseURl"]}/{source.ImageUrl}";
            }

            return string.Empty;
        }
    }


}
