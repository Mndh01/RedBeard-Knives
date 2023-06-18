using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class UserManagerExtension
    {
        public static async Task<AppUser> FindWithAddressByEmailAsync(this UserManager<AppUser> userManager, string email)
        {
            return await userManager.Users.Include(user => user.Addresses).SingleOrDefaultAsync(user => user.Email == email);
        }
    }
}