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
        Task<AddressDto> AddAddressAsync(AddressDto address, AppUser user);
        Task<bool> DeleteAddressAsync(Address address, AppUser user);
    }
}