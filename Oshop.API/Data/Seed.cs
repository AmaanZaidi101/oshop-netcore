using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Oshop.API.Models;

namespace Oshop.API.Data
{
    public class Seed
    {
        public static void SeedUsers(DataContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (!userManager.Users.Any())
            {
                var roles = new List<Role>
                {
                    new Role{Name = "Customer"},
                    new Role{Name = "Admin"},
                    new Role{Name = "Moderator"},
                    new Role{Name = "VIP"},
                };

                foreach (var role in roles)
                {
                    roleManager.CreateAsync(role).Wait();
                }

                var adminUser = new User
                {
                    UserName = "Admin",
                    Email = "syeda@mindfiresolutions.com",
                    DateOfBirth =  DateTime.Parse("1995/01/11"),
                    Created = DateTime.Now
                };

                IdentityResult result = userManager.CreateAsync(adminUser, "password").Result;

                if (result.Succeeded)
                {
                    var admin = userManager.FindByNameAsync("Admin").Result;
                    userManager.AddToRolesAsync(admin, new[] {"Admin", "Moderator", "Customer"}).Wait();
                }

                var categories = new List<Category>
                {
                    new Category{Name = "Bread"},
                    new Category{Name = "Dairy"},
                    new Category{Name = "Fruits"},
                    new Category{Name = "Seasonings and Spices"},
                    new Category{Name = "Vegetables"},
                };

                foreach (var category in categories)
                {
                    context.Categories.AddAsync(category).Wait();
                }

                context.SaveChangesAsync().Wait();

                var products = new List<Product>()
                {
                    new Product()
                    {
                        CategoryId = context.Categories.FirstOrDefault(x => x.Name == "Bread").Id,
                        Name = "Freshly Baked Bread",
                        Price = 2.0M,
                        ImageUrl = "https://cdn.pixabay.com/photo/2017/10/18/17/08/bread-2864792_960_720.jpg",
                        Weight = 200        
                    },
                    new Product()
                    {
                        CategoryId = context.Categories.FirstOrDefault(x => x.Name == "Dairy").Id,
                        Name = "Cheese",
                        Price = 4.0M,
                        ImageUrl = "https://p0.pikrepo.com/preview/183/44/yellow-cheese-on-red-plastic-tray.jpg",
                        Weight = 300
                    },
                    new Product()
                    {
                        CategoryId = context.Categories.FirstOrDefault(x => x.Name == "Fruits").Id,
                        Name = "Apple",
                        Price = 1.0M,
                        ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/e/ee/Apples.jpg",
                        Weight = 100     
                    },
                    new Product()
                    {
                        CategoryId = context.Categories.FirstOrDefault(x => x.Name == "Seasonings and Spices").Id,
                        Name = "Turmeric",
                        Price = 2.0M,
                        ImageUrl = "https://cdn.pixabay.com/photo/2017/08/08/23/19/spices-2613032_960_720.jpg",
                        Weight = 300     
                    },
                    new Product()
                    {
                        CategoryId = context.Categories.FirstOrDefault(x => x.Name == "Vegetables").Id,
                        Name = "Spinach",
                        Price = 1.0M,
                        ImageUrl = "https://cdn.pixabay.com/photo/2016/07/16/16/30/spinach-1522283_960_720.jpg",
                        Weight = 500       
                    }

                };

                foreach (var product in products)
                {
                    context.Products.AddAsync(product).Wait();
                }

                context.SaveChangesAsync().Wait();

            }
        }
    }
}