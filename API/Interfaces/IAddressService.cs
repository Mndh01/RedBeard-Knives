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
        bool AddressExistsForUserById(AppUser user, int addressId);
        Task<bool> DeleteAddressAsync(Address address);
        Task<bool> SaveAllAsync();
    }
}