using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Oshop.API.Models;

namespace Oshop.API.Data
{
    public class DataContext: IdentityDbContext<User, Role, string, 
        IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>, 
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public DataContext(DbContextOptions<DataContext> options): base (options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }
        public DbSet<Shipping> Shippings { get; set; }
        public DbSet<OrderShipping> OrderShippings { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserRole>(userRole => {
                userRole.HasKey(ur => new {ur.UserId, ur.RoleId});

                userRole.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

                userRole.HasOne(ur => ur.User)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            });

            builder.Entity<Order>().HasOne(x => x.OrderShipping)
            .WithOne(y => y.Order)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Order>()
            .HasMany(x => x.ProductItems)
            .WithOne(y => y.Order)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProductItem>()
            .HasOne(x => x.Product)
            .WithMany(y => y.ProductItems)
            .OnDelete(DeleteBehavior.Cascade);

            //  builder.Entity<Product>()
            //  .HasOne(x => x.ProductItem).WithOne(z => z.Product).IsRequired();
        }
    }
}