using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using API.Models;
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
            
            if (await userManager.Users.CountAsync() > 1) return;

            int i = 0;
            foreach (var user in users)
            {
                Address address = new Address
                {
                    AddressParts = "london_" + i + " ,uk",
                    IsCurrent = true,
                };

                UserAddresses newUserAddress = new UserAddresses
                {
                    Address = address,
                    User = user
                };
                
                user.UserName = user.UserName.ToLower();
                user.Email = "altair.2000nh"+ i +"@icloud.com";
                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Customer");
                _context.UserAddresses.Add(newUserAddress);
                user.UserAddresses.Add(newUserAddress);
                address.UserAddresses.Add(newUserAddress);

                i++;
            }

            await context.SaveChangesAsync();

        }
    }
}

// Try to create Admin user...
/* 

using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using API.Models;
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

            var admin = new AppUser
            {
                UserName = "Noar/Tom",
                Email = "altair.2000nh@icloud.com",

            };

            Address address1 = new Address
            {
                AddressParts = "london_0 ,uk",
                IsCurrent = true,
            };

            UserAddresses newUserAddress1 = new UserAddresses
            {
                Address = address1,
                User = admin
            };

            // List<string> list = new List<string>();
            // list.Add("Admin");
            // list.Add("Moderator");
            // IEnumerable<string> adminRoles = list;

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRoleAsync(admin, "Admin");
            _context.UserAddresses.Add(newUserAddress1);
            admin.UserAddresses.Add(newUserAddress1);
            address1.UserAddresses.Add(newUserAddress1);

            int i = 1;
            foreach (var user in users)
            {
                Address address = new Address
                {
                    AddressParts = "london_" + i + " ,uk",
                    IsCurrent = true,
                };

                UserAddresses newUserAddress = new UserAddresses
                {
                    Address = address,
                    User = user
                };
                
                user.UserName = user.UserName.ToLower();
                user.Email = "altair.2000nh"+ i +"@icloud.com";
                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Customer");
                _context.UserAddresses.Add(newUserAddress);
                user.UserAddresses.Add(newUserAddress);
                address.UserAddresses.Add(newUserAddress);

                i++;
            }

            await context.SaveChangesAsync();
        }
    }
}
    */