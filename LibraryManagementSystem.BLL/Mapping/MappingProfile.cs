using AutoMapper;
using LibraryManagementSystem.BLL.DTOs;
using LibraryManagementSystem.BLL.DTOs.AuthDTOs;
using LibraryManagementSystem.BLL.DTOs.ProductDTOs;
using LibraryManagementSystem.DAL.Entities;
using LibraryManagementSystem.DAL.Entities.IdentityEntities;
using LibraryManagementSystem.DAL.Entities.Products;

using LinkDev.Talabat.Core.Domain.Entities.Products;

namespace LinkDev.Talabat.Core.Application.Mapping
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductToReturnDTO>()
                .ForMember(d => d.Brand, o => o.MapFrom(src => src.Brand!.Name))
                .ForMember(d => d.Category, o => o.MapFrom(src => src.Category!.Name))
                .ForMember(d=>d.PictureUrl, o=>o.MapFrom<ProductPictureUrlResolver>());


            CreateMap<ProductBrand, BrandDTO>();
            CreateMap<ProductCategory, CategoryDTO>();
            CreateMap<Doctor, DoctorDto>().ReverseMap();
            CreateMap<Admin, AdminDto>().ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserDto>().ReverseMap();
            CreateMap<AddDepartmentDTO, Department>()
            .ForMember(dest => dest.DepName, opt => opt.MapFrom(src => src.Name)) // Mapping Name
            .ForMember(dest => dest.DepDescription, opt => opt.MapFrom(src => src.Dec)); 

            CreateMap<Department, DepartmentDTO>()
                       .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DepName))  // Ensure correct mapping
                       .ForMember(dest => dest.Desc, opt => opt.MapFrom(src => src.DepDescription));
            CreateMap<EditDepartmentDTO, Department>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DepName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.DepDescription, opt => opt.MapFrom(src => src.Desc));
            //CreateMap<CustomerBasket, CustomerBasketDTO>().ReverseMap();
            //CreateMap<BasketItem, BasketItemDTO>().ReverseMap();
        }
    }
}

