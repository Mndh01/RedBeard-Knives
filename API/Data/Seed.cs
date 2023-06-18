using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using API.Models;
using API.Models.OrderAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace API.Data
{

    // Not Tested After context + Default Addresses Were Added
    public class Seed
    {
        public static async Task SeedThings(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,DataContext context, IConfiguration config)
        {
            DataContext _context = context;
            
            IConfiguration _config = config;
            
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlite(_config.GetConnectionString("DefaultConnection"));
            _context = new DataContext(optionsBuilder.Options);

            if(roleManager.FindByIdAsync("1").Result == null)
            {
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

                await _context.SaveChangesAsync();
            }

            if (userManager.FindByNameAsync("Noar").Result == null)
            {
                var admin = new AppUser
                {
                    UserName = "Noar",
                    Email = "altair.2000nh@icloud.com",
                };

                var password = "Pa$$w0rd";

                var result = userManager.CreateAsync(admin, password).Result;

                if (result.Succeeded)
                {
                    await userManager.AddToRolesAsync(admin, new[] {"Admin", "Moderator"});
                }
            }
            
            if (!(await userManager.Users.CountAsync() > 1))
            {
                var userData = System.IO.File.ReadAllText("Data/SeedData/UserSeedData.json");
                var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

                int i = 0;
                foreach (var user in users)
                {
                    Models.Address address = new Models.Address
                    {
                        FullAddress = "london_" + i + " ,uk",
                        IsCurrent = true,
                        User = user
                    };
                    
                    user.UserName = user.UserName.ToLower();
                    user.Addresses.Add(address);
                    user.Email = "altair.2000nh"+ i +"@icloud.com";
                    var userResult = await userManager.CreateAsync(user, "Pa$$w0rd");
                    var roleResult = await userManager.AddToRoleAsync(user, "Customer");

                    i++;

                }
            }

            if(!await _context.DeliveryMethods.AnyAsync())
            {
                var DeliveryMethodsData = System.IO.File.ReadAllText("Data/SeedData/delivery.json");
                var Methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);
            
                foreach (var item in Methods)
                {
                    await _context.DeliveryMethods.AddAsync(item);
                }
                
                await _context.SaveChangesAsync();
            }


        }
    }
}