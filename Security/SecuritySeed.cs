using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Project.Models;

namespace Project.Security
{
    public class SecuritySeed
    {

        public static void SeedData(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,ApplicationDBContext context)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager,context);
        }

        private static void SeedUsers(UserManager<User> userManager, ApplicationDBContext context)
        {
            ShoppingCartRepository shoppingCart = new ShoppingCartRepository(context);            
           
            if(userManager.FindByNameAsync("Admin").Result == null)
            {
                var s = shoppingCart.AddEntity(new ShoppingCart());
                var user = new User
                {
                    Active = true,
                    City = "Habana",
                    Email = "admin@mail.com",
                    Id = "4",
                    Info = "Administrator",
                    LastName = "Admin",
                    Name = "Admin",
                    NormalizedEmail = "ADMIN@MAIL.COM",
                    NormalizedUserName = "ADMIN",
                    PhoneNumber = "12345678",
                    UserName = "Admin",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    LockoutEnabled = false,
                    ShoppingCartId = s.Id
                };
                try
                {
                    IdentityResult identityResult = userManager.CreateAsync(user, "Password123!").Result;
                    if (identityResult.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Admin").Wait();
                    }
                }
                catch (Exception e)
                {
                   var d = e.InnerException;
                }
            }
            if (userManager.FindByNameAsync("Moderator").Result == null)
            {
                var s2 = shoppingCart.AddEntity(new ShoppingCart());
                var moderator = new User
                {
                    Active = true,
                    City = "Habana",
                    Email = "moderator@mail.com",
                    Id = "5",
                    Info = "Moderator",
                    LastName = "Moderator",
                    Name = "Moderator",
                    NormalizedEmail = "MODERATOR@MAIL.COM",
                    NormalizedUserName = "MODERATOR",
                    PhoneNumber = "23456789",
                    UserName = "Moderator",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    LockoutEnabled = false,
                    ShoppingCartId = s2.Id
                };

                try
                {
                    IdentityResult identityResult = userManager.CreateAsync(moderator, "Password123!").Result;
                    if (identityResult.Succeeded)
                    {
                        userManager.AddToRoleAsync(moderator, "Moderator").Wait();
                    }
                }
                catch (Exception e)
                {
                    var d = e.InnerException;
                }
            }
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                role.NormalizedName = "ADMIN";
                IdentityResult identityResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Moderator").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Moderator";
                role.NormalizedName = "MODERATOR";
                IdentityResult identityResult = roleManager.CreateAsync(role).Result;
            }
        }

      

    }
}
