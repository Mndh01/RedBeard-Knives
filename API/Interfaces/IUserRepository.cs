using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Models;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDto>GetMemberAsync(int id);
        Task<IEnumerable<UserDto>>GetMembersAsync();
        Task<AppUser>GetUserByIdAsync(int id);
        Task<AppUser>GetUserByUsernameAsync(string username);
        Task<AppUser>GetUserByEmailAsync(string email);
        Task<IEnumerable<AppUser>>GetUsersAsync();
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
    }
}