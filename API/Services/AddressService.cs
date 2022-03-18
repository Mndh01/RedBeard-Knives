using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Interfaces;
using API.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class AddressService : IAddressService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public AddressService(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<AddressDto>> AddAddressAsync(AddressDto NewAddress, AppUser user)
        {
            Address address = new Address();

            _mapper.Map(NewAddress, address);
            

            UserAddresses userAddress = new UserAddresses
            {
                User = user,
                Address = address
            };

            var addressAdded = await _context.Addresses.AddAsync(address);

            var userAddressAdded = await _context.UserAddresses.AddAsync(userAddress);

            address.UserAddresses.Add(userAddress);


            return await _context.UserAddresses.Include(ua => ua.Address)
                    .Include(ua => ua.User)
                    .Where(ua => ua.UserId == user.Id)
                    .Select(ua => ua.Address)
                    .ProjectTo<AddressDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();
        }

        public Task<bool> DeleteAddressAsync(AddressDto address, AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<AddressDto> EditAddressAsync(AddressDto address, AppUser user)
        {
            throw new NotImplementedException();
        }

        // public async Task<bool> SetCurrentAddressAsync(int addressId, AppUser user)
        // {

        // }

        // public async Task<Address> GetAddressAsync(Address address)
        // {
        //     return await _context.Addresses.FirstOrDefaultAsync(x => x.Id == address.Id);
        // }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }        
    }
}