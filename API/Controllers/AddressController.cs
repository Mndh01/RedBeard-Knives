using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Extensions;
using API.Interfaces;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

namespace API.Controllers
{
    [Authorize]
    public class AddressController : BaseApiController
    {
        private readonly IAddressService _addressService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;
        public AddressController(DataContext context, UserManager<AppUser> userManager, 
            IAddressService addressService, IMapper mapper, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _addressService = addressService;
        }
        
        [HttpPost]
        public async Task<ActionResult<AddressDto>> AddAddress(AddressDto NewAddress)
        {
            var user = await _userRepository.GetUserByEmailAsync(User.GetEmail());
            
            if (user == null) return Unauthorized("Sing in first..");
            
            NewAddress.FullAddress = NewAddress.FullAddress.ToLower();

            NewAddress.FullAddress = RemoveWhitespace(NewAddress.FullAddress);
            
            if (user.UserAddresses.Count() > 0) NewAddress.IsCurrent = false;

            var existingAddress = user.UserAddresses.Select(ua => ua.Address).FirstOrDefault(a => a.FullAddress == NewAddress.FullAddress);
            
            if (existingAddress != null)
            {
                if (existingAddress.UserAddresses.Select(ua => ua.User).FirstOrDefault().Id == user.Id) return BadRequest("Address already exists..");
                
                UserAddresses address = new UserAddresses
                {
                    Address = existingAddress,
                    User = user
                };
                
                _mapper.Map(NewAddress, address.Address);

                var result_1 =await _addressService.AddAddressAsync(NewAddress, user);
                                
                if (result_1 != null) return CreatedAtRoute("get-user", new {id = user.Id} , result_1);

                return BadRequest("Failed to add address.");
            }

            var result_2 = await _addressService.AddAddressAsync(NewAddress, user);

            if (result_2 != null) return CreatedAtRoute("get-user", new {id = user.Id} , result_2);
            
            return BadRequest("Failed to add address..");
        }

        [HttpPut("edit-address")]
        public async Task<ActionResult<bool>> EditAddress(Address AddressToEdit)
        {
            var user = await _userRepository.GetUserByEmailAsync(User.GetEmail());

            if (user == null) return false;

            if (!_addressService.AddressExistsForUserById(user, AddressToEdit.Id)) return BadRequest("Address does not exist..");
            
            var address = user.UserAddresses.Select(ua => ua.Address).FirstOrDefault(a => a.Id == AddressToEdit.Id);

            address.FullAddress = AddressToEdit.FullAddress.ToLower();

            if (await _addressService.SaveAllAsync()) return Ok(address);

            return BadRequest("Failed to update the address..");
        }

        [HttpPut("set-current-address")]
        public async Task<ActionResult> SetCurrentAddress(int addressId)
        {
            var user = await _userRepository.GetUserByEmailAsync(User.GetEmail());

            var currentAddress = user.UserAddresses.Select(ua => ua.Address)
                        .SingleOrDefault(a => a.IsCurrent);

            var newCurrentAddress = user.UserAddresses.Select(ua => ua.Address)
                        .SingleOrDefault(a => a.Id == addressId);

            if (newCurrentAddress != null && newCurrentAddress.IsCurrent) return BadRequest("This is already your current address..");

            if(currentAddress != null) currentAddress.IsCurrent = false;

            newCurrentAddress.IsCurrent = true;

            if (await _addressService.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to change current address..");
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAddress(int addressId)
        {
            var user = await _userRepository.GetUserByEmailAsync(User.GetEmail());

            if (!_addressService.AddressExistsForUserById(user, addressId)) return BadRequest("Failed to find address to delete..");
            
            var address = user.UserAddresses.Select(ua => ua.Address)
                        .SingleOrDefault(a => a.Id == addressId);

            if (address.IsCurrent) return BadRequest("You can not remove your current address..");

            if (await _addressService.DeleteAddressAsync(address)) return NoContent();

            return BadRequest("Failed to remove address..");
        }
        public string RemoveWhitespace(string Input)
        {
            return String.Concat(Input.Where(c => !Char.IsWhiteSpace(c)));
        }
    }
}