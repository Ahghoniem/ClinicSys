using AutoMapper;
using LibraryManagementSystem.BLL.DTOs.ProductDTOs;
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
}
