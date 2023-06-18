using API.Models;
using API.Models.OrderAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

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
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Models.Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        public override int SaveChanges()
        {
            TrimStringProperties();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            TrimStringProperties();
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            if(Database.IsSqlite())
            {
                foreach (var entityType in builder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties()
                        .Where(p => p.PropertyType == typeof(decimal));
                    var dateTimeProperties = entityType.ClrType.GetProperties()
                    .Where(p => p.PropertyType == typeof(DateTimeOffset));

                    foreach (var property in properties)
                    {
                        builder.Entity(entityType.Name).Property(property.Name)
                            .HasConversion<double>();
                    }
                    
                    foreach (var property in dateTimeProperties)
                    {
                        builder.Entity(entityType.Name).Property(property.Name)
                            .HasConversion(new DateTimeOffsetToBinaryConverter());
                    }
                }
            }
            
            builder.Entity<AppUser>()
                .HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(u => u.UserId)
                .IsRequired();
        
            builder.Entity<AppRole>()
                .HasMany(r => r.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
            
            builder.Entity<AppUser>()
                .HasMany(u => u.Addresses)
                .WithOne(ua => ua.User)
                .HasForeignKey(ua => ua.UserId)
                .IsRequired();

            builder.Entity<AppUser>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .IsRequired();

            builder.Entity<Product>()
                .HasMany(p => p.Photos)
                .WithOne(p => p.Product)
                .IsRequired();
            
            builder.Entity<Photo>()
                .HasOne(p => p.Product)
                .WithMany(p => p.Photos)
                .HasForeignKey(p => p.ProductId)
                .IsRequired();
        }
        private void TrimStringProperties()
        {
            var stringProperties = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .SelectMany(e => e.Properties)
                .Where(p => p.Metadata.ClrType == typeof(string));

            foreach (var property in stringProperties)
            {
                if (property.CurrentValue is string value)
                {
                    property.CurrentValue = value.Trim();
                }
            }
        }
    }
}