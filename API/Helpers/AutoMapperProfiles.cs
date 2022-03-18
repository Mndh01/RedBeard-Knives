using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Models;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            
            CreateMap<Product, ItemDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
                    src.Photos.FirstOrDefault(x => x.IsMain).Url));
            CreateMap<Photo, PhotoDto>();
            CreateMap<ProductUpdateDto, Product>();
            CreateMap<RegisterDto, AppUser>();
            CreateMap<AddressDto, Address>().ReverseMap();
            CreateMap<AppUser, MemberDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photo.Url))
                .ForMember(dest => dest.Addresses,opt => opt.MapFrom(src => src.UserAddresses.Select(ua => ua.Address)))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => DateTime.Now.Year - src.DateOfBirth.Year));
        }
        
    }
}