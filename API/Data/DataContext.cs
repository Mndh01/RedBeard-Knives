using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
        IdentityUserClaim<int>,AppUserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<Product> Products { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<UserAddresses> UserAddresses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        
            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
            
            builder.Entity<AppUser>()
                .HasMany(ua => ua.UserAddresses)
                .WithOne(u => u.User)
                .HasForeignKey(ua => ua.UserId)
                .IsRequired();

            builder.Entity<Address>()
                .HasMany(ua => ua.UserAddresses)
                .WithOne(a => a.Address)
                .HasForeignKey(ua => ua.AddressId)
                .IsRequired();

            builder.Entity<UserAddresses>()
                .HasKey(k => new {k.AddressId, k.UserId});

            // builder.Entity<AppUser>()
            //     .HasOne(u => u.Photo)
            //     .WithOne<AppUser>();

        }
    }
}