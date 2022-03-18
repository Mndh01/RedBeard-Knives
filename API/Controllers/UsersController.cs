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
    public class UsersController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly SignInManager<AppUser> _signInManager;
        public UsersController(DataContext context, IUserRepository userRepository,
                UserManager<AppUser> userManager, IPhotoService photoService, IMapper mapper,
                SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
            _userRepository = userRepository;
            _userManager = userManager;
            _context = context;
            _photoService = photoService;
            _mapper = mapper;

        }

        [Authorize]
        [HttpGet("get-users")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {

            var result = await _userRepository.GetMembersAsync();

            if (result != null) return Ok(result);

            return BadRequest("Failed to fetch users..");
        }

        [HttpGet("get-user/{id}", Name = "get-user")]
        public async Task<ActionResult<MemberDto>> GetUserById(int id)
        {
            var result = await _userRepository.GetMemberAsync(id);

            if (result != null) return Ok(result);

            return BadRequest("Failed to find user..");
        }

        [HttpPut("change-photo")]
        public async Task<ActionResult<bool>> ChangePhoto(IFormFile newPhoto)
        {
            if (newPhoto == null) return BadRequest("Invalid file..");

            var user = await _userManager.FindByEmailAsync(User.GetEmail());
            
            if (user == null) return BadRequest("Failed to find user to change their photo..");

            if (user.Photo?.PublicId != null) {
                
                var deleteResult = await _photoService.DeletePhotoAsync(user.Photo.PublicId);
                
                if(deleteResult.Error != null) return BadRequest(deleteResult.Error.Message);
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
            
            _userRepository.Update(user);
            // var updateResult = await _userManager.UpdateAsync(user);
            
            // UserPhotoDto photoToReturn = new UserPhotoDto{};

            // _mapper.Map(user.Photo, photoToReturn);
            // if (updateResult.Errors.Count() > 0) await _photoService.DeletePhotoAsync(photo.PublicId);

            if (await _userRepository.SaveAllAsync()) return Ok(photo);

            return BadRequest("Failed to update photo..");
        }

    }
}