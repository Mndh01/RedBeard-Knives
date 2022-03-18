using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Models;

namespace API.Interfaces
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressDto>> AddAddressAsync(AddressDto address, AppUser user);
        Task<AddressDto> EditAddressAsync(AddressDto address, AppUser user);
        // Task<AddressDto> SetCurrentAddressAsync(AddressDto address, AppUser user);
        Task<bool> DeleteAddressAsync(AddressDto address, AppUser user);
        Task<bool> SaveAllAsync();
    }
}