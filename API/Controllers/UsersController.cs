using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Extensions;
using API.Interfaces;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        public UsersController(DataContext context, IUserRepository userRepository,
            UserManager<AppUser> userManager, IPhotoService photoService, IMapper mapper,
            ITokenService tokenService)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _userManager = userManager;
            _context = context;
            _photoService = photoService;
            _mapper = mapper;

        }

        [Authorize(Policy="RequireAdminRole")]
        [HttpGet("get-users")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {

            var result = await _userRepository.GetMembersAsync();

            if (result != null) return Ok(result);

            return BadRequest("Failed to fetch users..");
        }

        [Authorize]
        [HttpGet("get-user/{id}", Name = "get-user")]
        public async Task<ActionResult<MemberDto>> GetUserById(int id)
        {
            var result = await _userRepository.GetMemberAsync(id);

            if (result != null) return Ok(result);

            return BadRequest("Failed to find user..");
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userRepository.GetUserByEmailAsync(User?.GetEmail());

            return new UserDto
            {
                Email = user.Email,
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };
        }
        
        [HttpPut("change-photo")]
        public async Task<ActionResult<bool>> ChangePhoto(IFormFile newPhoto)
        {
            if (newPhoto == null) return BadRequest("Invalid file..");

            var user = await _userRepository.GetUserByEmailAsync(User.GetEmail());
            
            if (user == null) return BadRequest("Failed to find user to change their photo..");

            if (user.Photo?.PublicId != null) {
                
                var deleteResult = await _photoService.DeletePhotoAsync(user.Photo.PublicId);
                
                if(deleteResult.Error != null) return BadRequest(deleteResult.Error.Message);

                _context.Remove(user.Photo);
            }

            var result = await _photoService.AddPhotoAsync(newPhoto);
            
            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new UserPhoto 
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                UserId = user.Id
            };
            
            user.Photo = photo;
            
            if (await _userRepository.SaveAllAsync()) return Ok(photo);

            await _photoService.DeletePhotoAsync(photo.PublicId);

            return BadRequest("Failed to update photo..");
        }

    }
}