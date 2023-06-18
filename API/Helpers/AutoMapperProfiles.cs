using System;
using System.Linq;
using API.DTOs;
using API.DTOs.Admin;
using API.Models;
using API.Models.OrderAggregate;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToLower()))
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => 
                !String.IsNullOrEmpty(src.PhotoUrl) ? src.PhotoUrl 
                : src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ReverseMap();
            
            CreateMap<ProductUpdateDto, Product>();
            CreateMap<Photo, PhotoDto>().ReverseMap();

            CreateMap<Review, ReviewDto>().ReverseMap();
                        
            CreateMap<Models.Address, AddressDto>().ReverseMap();
            CreateMap<AddressDto, Models.OrderAggregate.Address>();
            
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ItemOrdered.ProductItemId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ItemOrdered.ProductName))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.ItemOrdered.PictureUrl));
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(dest => dest.DeliveryMethod, opt => opt.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.ShippingPrice, opt => opt.MapFrom(src => src.DeliveryMethod.Price));
            CreateMap<Order, OrderForAdminDto>()
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate.DateTime.ToString("dd/MM/yyyy HH:mm")));
                
            CreateMap<RegisterDto, AppUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.FirstName));
            CreateMap<AppUser, UserDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photo.Url))
                .ReverseMap();
            CreateMap<AppUser, UserForAdminDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name)))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToString("dd/MM/yyyy")));
        }
    }
}