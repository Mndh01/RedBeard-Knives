using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Extensions;
using API.Interfaces;
using API.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class AddressService : IAddressService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public AddressService(IMapper mapper, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<AddressDto> AddAddressAsync(AddressDto address, AppUser user)
        {            
            user.Addresses.Add(_mapper.Map<Address>(address));

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return address;
            
            return null;
        }

        public async Task<bool> DeleteAddressAsync(Address address, AppUser user)
        {
            user.Addresses.Remove(address);

            var result = await _userManager.UpdateAsync(user); 

            if (result.Succeeded) return true;

            return false;
        } 
    }
}