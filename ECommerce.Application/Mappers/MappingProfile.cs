using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Application.DTOs.AccountDTOs;
using ECommerce.Application.DTOs.BasketDTOs;
using ECommerce.Application.DTOs.CategoryDTOs;
using ECommerce.Application.DTOs.CustomerDTOs;
using ECommerce.Application.DTOs.OrderDTOs;
using ECommerce.Application.DTOs.ProductDTOs;
using ECommerce.Application.DTOs.SellerDTOs;
using ECommerce.Application.DTOs.UserDTOs;
using ECommerce.Application.VMs.Account;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Category mappings
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<CategoryCreateDTO, Category>();
            CreateMap<CategoryEditDTO, Category>();
            
            // User mappings
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserUpdateDTO>();
            CreateMap<UserCreateDTO, User>();
            CreateMap<UserUpdateDTO, User>();
            CreateMap<User, UserDTO>()
    .ForMember(dest => dest.Role, opt => opt.Ignore());

            // Product mappings
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.ImageFile, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.ImagePath, opt => opt.Ignore());
            CreateMap<ProductCreateDTO, Product>();
            CreateMap<Product, ProductEditDTO>().ReverseMap();
            CreateMap<ProductDTO, ProductEditDTO>().ReverseMap();


            // Order mappings
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
            CreateMap<OrderUpdateDTO, Order>();
            
            // Account mappings
            CreateMap<LoginVM, LoginDTO>().ReverseMap();
            
            // Seller mappings
            CreateMap<Seller, SellerDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName ?? ""))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email ?? ""))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName ?? ""))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName ?? ""))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName ?? ""))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address ?? ""))
                .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => src.LogoUrl ?? "")).ReverseMap();
            CreateMap<SellerCreateDTO, Seller>();
            CreateMap<SellerUpdateDTO, Seller>();

            CreateMap<RegisterDTO, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
            CreateMap<RegisterVM, RegisterDTO>();


            CreateMap<Customer, CustomerDetailDTO>().ReverseMap();

            CreateMap<CartItem, CartItemDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Product.ImagePath))
                .ReverseMap();
        }
    }
}
