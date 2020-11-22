using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    static public class Seed
    {
        static public void AddData(ApplicationDBContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (context.Database.GetPendingMigrations().Any())
                context.Database.Migrate();
            else return;

            context.ShoppingCarts.Add(new ShoppingCart { });
            context.ShoppingCarts.Add(new ShoppingCart { });
            context.ShoppingCarts.Add(new ShoppingCart { });
            context.ShoppingCarts.Add(new ShoppingCart { });
            context.ShoppingCarts.Add(new ShoppingCart { });
            context.SaveChanges();
            List<long> sh = context.ShoppingCarts.Select(p => p.Id).ToList();

            context.Users.Add(new User
            {
                Id = "1",
                Name = "Adrian",
                LastName = "Hernandez",
                UserName = "AHernandez",
                Info = "Los mejores productos",
                City = "San Miguel, La Habana",
                PhoneNumber = "55123456",
                Email = "a.hernandez@mail.com",
                ShoppingCartId = sh[0]
            });
            context.Users.Add(new User
            {
                Id = "2",
                Name = "Claudia",
                LastName = "Olavarrieta",
                UserName = "Claudita",
                Info = "Los mejores productos",
                City = "Regla, La Habana",
                PhoneNumber = "54321678",
                Email = "c.olavarrieta@mail.com",
                ShoppingCartId = sh[1]
            });
            context.Users.Add(new User
            {
                Id = "3",
                Name = "Marcos",
                LastName = "Valdivie",
                UserName = "marcOS",
                Info = "Los mejores productos",
                City = "Cienfuegos",
                PhoneNumber = "54781698",
                Email = "m.valdivie@mail.com",
                ShoppingCartId = sh[2]
            });
            var admin = new User
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
                ShoppingCartId = sh[3]
            };
            userManager.CreateAsync(admin, "Password123!").Wait();
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
                ShoppingCartId = sh[4]
            };
            userManager.CreateAsync(moderator, "Password123!").Wait();

            context.Products.Add(new Product
            {
                Name = "Shoe",
                Price = 34.5M,
                Images = "main-product01.jpg",
                Rating = 3,
                Ammount = 5,
                Category = Category.Shoes,
                UserId = "1",
                Active = true
            });
            context.SaveChanges();
            context.Products.Add(new Product
            {
                Name = "Brown Bag",
                Price = 20.5M,
                Images = "banner01.jpg",
                Rating = 4,
                Category = Category.Accesories,
                UserId = "2",
                Active = true,
                Ammount = 3
            });
            context.SaveChanges();
            context.Products.Add(new Product
            {
                Name = "Wallet",
                Price = 10.5M,
                Images = "product03.jpg",
                Rating = 5,
                Category = Category.Accesories,
                UserId = "3",
                Active = true,
                Ammount = 2
            });
            context.SaveChanges();
            context.Products.Add(new Product
            {
                Name = "Blue Shoe",
                Price = 34.5M,
                Images = "product04.jpg",
                Rating = 3,
                Category = Category.Shoes,
                UserId = "1",
                Active = true,
                Ammount = 8
            });
            context.SaveChanges();
            context.Products.Add(new Product
            {
                Name = "Black Heels",
                Price = 39.5M,
                Images = "product05.jpg",
                Rating = 4,
                Category = Category.Shoes,
                UserId = "2",
                Active = true,
                Ammount = 1
            });
            context.SaveChanges();
            context.Products.Add(new Product
            {
                Name = "Work Bag",
                Price = 15M,
                Images = "product06.jpg",
                Rating = 5,
                Category = Category.Accesories,
                UserId = "3",
                Active = true,
                Ammount = 4
            });
            context.SaveChanges();
            List<long> pid = context.Products.Select(p => p.Id).ToList();

            context.Reviews.Add(new Review
            {
                UserId = "1",
                ProductId = pid[1],
                ReviewText = "Lorem ipsum dolor sit amet, consectetur adipisicing elit," +
            " sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam," +
            " quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                Rating = 3,
                Date = new DateTime(2019, 12, 26)
            });
            context.Reviews.Add(new Review
            {
                UserId = "1",
                ProductId = pid[2],
                ReviewText = "Lorem ipsum dolor sit amet, consectetur adipisicing elit," +
            " sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam," +
            " quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                Rating = 4,
                Date = new DateTime(2019, 12, 13)
            });
            context.Reviews.Add(new Review
            {
                UserId = "2",
                ProductId = pid[1],
                ReviewText = "Lorem ipsum dolor sit amet, consectetur adipisicing elit," +
            " sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam," +
            " quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                Rating = 5,
                Date = new DateTime(2020, 1, 7)
            });
            context.Reviews.Add(new Review
            {
                UserId = "1",
                ProductId = pid[5],
                ReviewText = "Lorem ipsum dolor sit amet, consectetur adipisicing elit," +
            " sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam," +
            " quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                Rating = 1,
                Date = new DateTime(2019, 12, 26)
            });
            context.Reviews.Add(new Review
            {
                UserId = "2",
                ProductId = pid[3],
                ReviewText = "Lorem ipsum dolor sit amet, consectetur adipisicing elit," +
            " sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam," +
            " quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                Rating = 2,
                Date = new DateTime(2020, 2, 10)
            });
            context.Reviews.Add(new Review
            {
                UserId = "3",
                ProductId = pid[0],
                ReviewText = "Lorem ipsum dolor sit amet, consectetur adipisicing elit," +
            " sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam," +
            " quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                Rating = 3,
                Date = new DateTime(2019, 12, 26)
            });
            context.Reviews.Add(new Review
            {
                UserId = "1",
                ProductId = pid[4],
                ReviewText = "Lorem ipsum dolor sit amet, consectetur adipisicing elit," +
            " sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam," +
            " quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                Rating = 3,
                Date = new DateTime(2019, 12, 26)
            });

            context.Auctions.Add(new Auction
            {
                Active = true,
                ActualPrice = 14,
                ActualUser = "2",
                Ammount = 1,
                Date = new DateTime(2020, 12, 24),
                InitialPrice = 14,
                InitialTime = new DateTime(2020, 07, 15),
                ProductId = pid[5],
                User_Sale_ID = "3",
            });

            context.BankAccounts.Add(new BankAccount
            {
                AccountId = "88888888",
                Titular = "Shoppify",
                Ammount = 0.1F
            });

            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                role.NormalizedName = "ADMIN";
               roleManager.CreateAsync(role).Wait();
            }
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Moderator";
                role.NormalizedName = "MODERATOR";
                roleManager.CreateAsync(role).Wait();
            }

            userManager.AddToRoleAsync(admin, "Admin").Wait();
            userManager.AddToRoleAsync(moderator, "Moderator").Wait();

            context.SaveChanges();
        }
    }
}

