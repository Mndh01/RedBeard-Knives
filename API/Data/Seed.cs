using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedThings(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            if (users == null) return;

            var roles = new List<AppRole>
            {
                new AppRole{Name = "Customer"},
                new AppRole{Name = "Moderator"},
                new AppRole{Name = "Admin"}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            int i = 0;
            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();
                user.Email = "altair.2000nh"+ i +"@icloud.com";
                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Customer");
                i++;
            }

            var admin = new AppUser
            {
                UserName = "admin"
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] {"Admin", "Moderator"});

        }
    }
}