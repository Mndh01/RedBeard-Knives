using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Extensions;
using API.Interfaces;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Authorize]
    public class AddressController : BaseApiController
    {
        private readonly IAddressService _addressService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        public AddressController(UserManager<AppUser> userManager, IAddressService addressService, IMapper mapper)
        {
            _mapper = mapper;
            _userManager = userManager;
            _addressService = addressService;
        }

        [HttpGet(Name = "get-address")]
        public async Task<ActionResult<Address>> GetAddress(int id)
        {
            var user = await _userManager.FindWithAddressByEmailAsync(User.GetEmail());

            var address = user.Addresses.FirstOrDefault(a => a.Id == id);

            if (address != null) return Ok(address);

            return BadRequest("Address does not exist");
        }

        
        [HttpGet("all-addresses")]
        public async Task<ActionResult<AddressDto>> GetAddresses()
        {
            var user = await _userManager.FindWithAddressByEmailAsync(User.GetEmail());
            
            return Ok(_mapper.Map<Address[], AddressDto[]>(user.Addresses.ToArray()));
        }
        
        
        [HttpPost]
        public async Task<ActionResult<AddressDto>> AddAddress(AddressDto address)
        {
            var user = await _userManager.FindWithAddressByEmailAsync(User.GetEmail());
                                    
            LowerAddressAndConc(address);

            if (user.Addresses.Count == 0) address.IsCurrent = true;
            
            if(address.IsCurrent && user.Addresses.Count > 0) user.Addresses.SingleOrDefault(a => a.IsCurrent).IsCurrent = false;
            
            var addressExists = user.Addresses.Any(a => a.FullAddress == address.FullAddress);

            if (addressExists) return BadRequest("Address already exists");

            var addressNameExists = user.Addresses.Any(a => a.DisplayName == address.DisplayName);

            if (addressNameExists) return BadRequest("Address display name already in use, try another one");

            var result = await _addressService.AddAddressAsync(address, user);

            if (result != null) return CreatedAtRoute("get-address", new {id = address.Id} , result);
            
            return BadRequest("Failed to add address");
        }

        [HttpPut]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto updatedAddress)
        {
            var user = await _userManager.FindWithAddressByEmailAsync(User.GetEmail());

            var address = user.Addresses.FirstOrDefault(a => a.FullAddress == updatedAddress.FullAddress);
            
            if (address == null) return BadRequest("Address does not exist..");

            LowerAddressAndConc(updatedAddress);

            _mapper.Map(updatedAddress, address);

            var result = await _userManager.UpdateAsync(user);
            
            if (result.Succeeded) return NoContent();

            return BadRequest("Failed to update the address..");
        }

        [HttpPut("set-current-address")]
        public async Task<ActionResult> SetCurrentAddress([FromBody] int id)
        {
            var user = await _userManager.FindWithAddressByEmailAsync(User.GetEmail());

            var newCurrentAddress = user.Addresses.SingleOrDefault(a => a.Id == id);

            if (newCurrentAddress == null) return BadRequest("Address does not exist");
            
            if (newCurrentAddress.IsCurrent) return BadRequest("This is already your current address");

            user.Addresses.SingleOrDefault(a => a.IsCurrent).IsCurrent = false;

            newCurrentAddress.IsCurrent = true;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return NoContent();

            return BadRequest("Failed to change current address..");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAddress(int id)
        {
            var user = await _userManager.FindWithAddressByEmailAsync(User.GetEmail());

            var address = user.Addresses.FirstOrDefault(a => a.Id == id);
            
            if (address == null) return BadRequest("Failed to find address to delete..");

            if (address.IsCurrent) return BadRequest("You can not remove your current address..");

            if (await _addressService.DeleteAddressAsync(address, user)) return NoContent();

            return BadRequest("Failed to remove address..");
        }
    }
}