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
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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

        
        [HttpPost("add-address")]
        public async Task<ActionResult<AddressDto>> AddAddress(AddressDto NewAddress)
        {
            var user = await _userManager.FindByEmailAsync(User.GetEmail());
            
            if (user == null) return Unauthorized("Sing in first..");
            
            NewAddress.AddressParts = NewAddress.AddressParts.ToLower();

            NewAddress.AddressParts = RemoveWhitespace(NewAddress.AddressParts);
            
            NewAddress.IsCurrent = false;

            var existingAddress = await _context.UserAddresses.Where(x => x.Address.AddressParts == NewAddress.AddressParts).FirstOrDefaultAsync();
            
            if (existingAddress != null)
            {
                if (existingAddress.UserId == user.Id) return BadRequest("Address already exists..");
                
                UserAddresses address = new UserAddresses
                {
                    Address = existingAddress.Address,
                    User = user
                };
                
                _mapper.Map(NewAddress, address.Address);
                                
                await _context.UserAddresses.AddAsync(address);
                
                if (await _addressService.SaveAllAsync()) return CreatedAtRoute("get-user", new {id = user.Id} , address);

                return BadRequest("Failed to add address.");
            } 

            var userAddresses = await _addressService.AddAddressAsync(NewAddress, user);

            if (userAddresses == null) return BadRequest("Failed to add address..");

            if (await _addressService.SaveAllAsync()) return CreatedAtRoute("get-user", new {id = user.Id} , userAddresses);
            
            return BadRequest("Failed to add address.");
        }

        [HttpPut("edit-address")]
        public async Task<ActionResult<bool>> EditAddress(Address AddressToEdit)
        {
            var user = await _userRepository.GetUserByEmailAsync(User.GetEmail());

            if (user == null) return false;

            var address = user.UserAddresses.Select(ua => ua.Address).FirstOrDefault(a => a.Id == AddressToEdit.Id);
                        
            if (address == null) return BadRequest("Address does not exist..");

            address.AddressParts = AddressToEdit.AddressParts.ToLower();

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

        public string RemoveWhitespace(string Input)
        {
            return String.Concat(Input.Where(c => !Char.IsWhiteSpace(c)));
        }
    }
}