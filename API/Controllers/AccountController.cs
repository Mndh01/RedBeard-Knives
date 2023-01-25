using System;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Extensions;
using API.Interfaces;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly DataContext _context;
        private readonly IPhotoService _photoService;
        public AccountController(DataContext context, UserManager<AppUser> userManager,
         SignInManager<AppUser> signInManager, ITokenService tokenService,
         IMapper mapper, IPhotoService photoService)
        {
            _photoService = photoService;
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(await UserExists(registerDto.Email)) return BadRequest("E-mail is already in use.");

            registerDto.Address.IsCurrent = true;
            registerDto.Username = registerDto.FirstName + "_" + registerDto.SureName;

            LowerAddressAndConc(registerDto.Address);
            
            var user = _mapper.Map<AppUser>(registerDto);

            user.UserName = user.UserName.ToLower();

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Customer");

            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            var addressExists = await _context.Addresses.FirstOrDefaultAsync(a => a.FullAddress == registerDto.Address.FullAddress);

            if (addressExists == null) 
            {
                await _context.Addresses.AddAsync(registerDto.Address);    

                await _context.SaveChangesAsync();
            }

            var address = await _context.Addresses.FirstOrDefaultAsync(x => x.FullAddress == registerDto.Address.FullAddress);

            var userAddress = new UserAddresses
            {
                User = user,
                Address = address,
            };
            
            await _context.UserAddresses.AddAsync(userAddress);

            await _context.SaveChangesAsync();

            var addressToAssign = new AddressDto{};

            _mapper.Map(address, addressToAssign);

            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                PhotoUrl = user.Photo?.Url,
                Email = user.Email,
                Gender = user.Gender,
                Address = addressToAssign
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .Include(p => p.Photo)
                .Include(a => a.UserAddresses)
                .ThenInclude(ua => ua.Address)
                .SingleOrDefaultAsync(x => x.Email == loginDto.Email);

            if(user == null) return Unauthorized("Invalid Email..");

            var address = user.UserAddresses.Select(ua => ua.Address).SingleOrDefault(a => a.IsCurrent == true);

            var result = await _signInManager
                .CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Wrong password..");

            var addressToAssign = new AddressDto{};

            _mapper.Map(address, addressToAssign);

            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                PhotoUrl = user.Photo?.Url,
                Email = user.Email,
                Gender = user.Gender,
                Address = addressToAssign
            };
        }
        
        [HttpGet("email-exists")]
        private async Task<ActionResult<bool>> EmailExists([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [Authorize]
        [HttpDelete("delete-user")]
        public async Task<ActionResult> DeleteUser()
        {
            var user = await _userManager.FindByEmailAsync(User.GetEmail());
            
            if (user == null) return Unauthorized("Sing in first..");

            if (user.Photo?.Url != null) 
            {
                var photoDeleteResult = await _photoService.DeletePhotoAsync(user.Photo.PublicId);
            
                if (photoDeleteResult.Error == null) return BadRequest("Failed to remove user because their photo was not removed.. \n\n" +  photoDeleteResult.Error?.Message);
            }

            // await _signInManager.SignOutAsync(); Is it neccessary though ?? :/
            
            var result = await _userManager.DeleteAsync(user);
            
            if (result.Errors.Count() == 0) return NoContent();

            return BadRequest("Failed to remove user.. ");
        }

        private async Task<bool> UserExists(string Email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == Email);
        }

        private void LowerAddressAndConc(Address address)
        {
            var properties = from p in typeof(Address).GetProperties()
                         where p.PropertyType == typeof(string) &&
                               p.CanRead &&
                               p.CanWrite &&
                               p.Name != "AddressParts"
                         select p;
        
            foreach (var property in properties)
            {
                var value = (string)property.GetValue(address, null);
                if (value != null)
                {   
                    value = value.ToLower();
                    property.SetValue(address, value, null);
                    address.FullAddress += value + " ";
                }
            }
        }
    }
}