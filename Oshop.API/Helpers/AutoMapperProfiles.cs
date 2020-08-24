using System.Linq;
using AutoMapper;
using Oshop.API.Dtos;
using Oshop.API.Models;

namespace Oshop.API.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<Product, ProductDto>().ReverseMap();

            CreateMap<ProductItem, ProductItemDto>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));

            CreateMap<ProductItemDto, ProductItem>()
                .ForMember(dest => dest.Product, opt => opt.Ignore());

            CreateMap<CartItem, CartItemDto>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));

            CreateMap<CartItemDto, CartItem>()
                .ForMember(dest => dest.Product, opt => opt.Ignore());

            CreateMap<Cart, CartDto>()
                .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems));
            
            CreateMap<CartDto, Cart>();

            CreateMap<User, UserForDetailedDto>().ReverseMap();

            CreateMap<User, UserForRegisterDto>().ReverseMap();

            CreateMap<User,UserForRolesDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(x => x.Role.Name)));

            CreateMap<Shipping, ShippingDto>();

            CreateMap<ShippingDto, Shipping>()
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<OrderDto, Order>().ReverseMap();

            CreateMap<OrderShipping, OrderShippingDto>().ReverseMap();
        }
    }
}