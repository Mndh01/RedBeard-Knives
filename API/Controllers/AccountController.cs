using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Extensions;
using API.Interfaces;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IPhotoService _photoService;
        public AccountController(UserManager<AppUser> userManager,
         SignInManager<AppUser> signInManager, ITokenService tokenService,
         IMapper mapper, IPhotoService photoService)
        {
            _photoService = photoService;
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        
        [HttpGet("email-exists")]
        private async Task<bool> EmailExists([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
        
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailAsync(User?.GetEmail());

            UserDto currentUser = _mapper.Map<UserDto>(user);

            currentUser.Token = await _tokenService.CreateToken(user);
            
            return Ok(currentUser);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(await EmailExists(registerDto.Email)) return BadRequest("E-mail is already in use.");

            registerDto.Address.IsCurrent = true;

            LowerAddressAndConc(registerDto.Address);
            
            AppUser user = _mapper.Map<AppUser>(registerDto);

            user.Addresses.Add(_mapper.Map<Address>(registerDto.Address));

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Customer");

            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);
            
            return new UserDto
            {
                UserName = user.UserName,
                SureName = user.SureName,
                Token = await _tokenService.CreateToken(user),
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                PhotoUrl = user.Photo?.Url,
                DateOfBirth = user.DateOfBirth,
                Addresses = _mapper.Map<ICollection<AddressDto>>(user.Addresses)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindWithAddressByEmailAsync(loginDto.Email);

            if(user == null) return Unauthorized("Your Email or Password is invalid");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Your Email or Password is invalid");

            return new UserDto
            {
                UserName = user.UserName,
                SureName = user.SureName,
                Token = await _tokenService.CreateToken(user),
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                PhotoUrl = user.Photo?.Url,
                DateOfBirth = user.DateOfBirth,
                Addresses = _mapper.Map<ICollection<AddressDto>>(user.Addresses)
            };
        }

        [Authorize]
        [HttpPut("change-password")]
        public async Task<ActionResult<bool>> ChangePassword(PasswordChangeDto passwordChangeDto)
        {
            var user = await _userManager.FindByEmailAsync(User.GetEmail());

            if (user == null) return Unauthorized("Sing in first..");
            
            if (passwordChangeDto.NewPassword != passwordChangeDto.NewPasswordConfirm) return BadRequest("Passwords do not match");

            var result = await _userManager.ChangePasswordAsync(user, passwordChangeDto.OldPassword, passwordChangeDto.NewPassword);

            if (result.Succeeded) return Ok(true);

            return BadRequest(result.Errors);
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
            
                if (photoDeleteResult.Error == null) return BadRequest("User photo was not removed, user removal canceled \n\n" +  photoDeleteResult.Error?.Message);
            }
            // TODO: Remove all user orders and addresses
            // await _signInManager.SignOutAsync(); Is it neccessary though ?? :/
            
            var result = await _userManager.DeleteAsync(user);
            
            if (result.Errors.Count() == 0) return NoContent();

            return BadRequest("Failed to remove user.. ");
        }
    }
}