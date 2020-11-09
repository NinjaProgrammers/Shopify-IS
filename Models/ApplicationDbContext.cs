using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class ApplicationDBContext : IdentityDbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductInCart>().HasKey(t => new { t.ProductId, t.ShoppingCartId });
            modelBuilder.Entity<Review>().HasKey(t => new { t.ProductId, t.UserId });
            modelBuilder.Entity<UserInAuction>().HasKey(t => new { t.AuctionId, t.UserId });

            foreach (var fk in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            modelBuilder.Entity<Review>()
                .HasOne(pd => pd.Product)
                .WithMany(r => r.Reviews)
                .HasForeignKey(pd => pd.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
               

            modelBuilder.Entity<Review>()
                .HasOne(pd => pd.User)
                .WithMany(r => r.Reviews)
                .HasForeignKey(pd => pd.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Sale>()
                .HasOne(pd => pd.User_Buy)
                .WithMany(r => r.Buys)
                .HasForeignKey(pd => pd.User_Buy_ID)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Sale>()
                 .HasOne(pd => pd.User_Sale)
                 .WithMany(r => r.Sales)
                 .HasForeignKey(pd => pd.User_Sale_ID)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductInCart>()
                .HasOne(pd => pd.Product)
                .WithMany(r => r.ProductInCars)
                .HasForeignKey(pd => pd.ProductId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<ProductInCart>()
                .HasOne(pd => pd.SCart)
                .WithMany(r => r.ProductInCars)
                .HasForeignKey(pd => pd.ShoppingCartId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserInAuction>()
               .HasOne(pd => pd.User)
               .WithMany(r => r.UsersInAuctions)
               .HasForeignKey(pd => pd.UserId)
               .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserInAuction>()
              .HasOne(pd => pd.Auction)
              .WithMany(r => r.UsersInAuctions)
              .HasForeignKey(pd => pd.AuctionId)
              .OnDelete(DeleteBehavior.Restrict);

            SeedData(modelBuilder);

        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShoppingCart>().HasData(
                new ShoppingCart { Id = 1 },
                new ShoppingCart { Id = 2 },
                new ShoppingCart { Id = 3 },
                new ShoppingCart { Id = 4 },
                new ShoppingCart { Id = 5 }
                );

            
            

            modelBuilder.Entity<User>().HasData( 
                new User
                {
                    Id = "1",
                    Name = "Adrian",
                    LastName = "Hernandez",
                    UserName = "AHernandez",
                    Info = "Los mejores productos",
                    City = "San Miguel, La Habana",
                    PhoneNumber = "55123456",
                    Email = "a.hernandez@mail.com",
                    ShoppingCartId = 1

                },
                 new User
                 {
                     Id = "2",
                     Name = "Claudia",
                     LastName = "Olavarrieta",
                     UserName = "Claudita",
                     Info = "Los mejores productos",
                     City = "Regla, La Habana",
                     PhoneNumber = "54321678",
                     Email = "c.olavarrieta@mail.com",
                     ShoppingCartId = 2
                 },
                 new User
                 {
                     Id = "3",
                     Name = "Marcos",
                     LastName = "Valdivie",
                     UserName = "marcOS",
                     Info = "Los mejores productos",
                     City = "Cienfuegos",
                     PhoneNumber = "54781698",
                     Email = "m.valdivie@mail.com",
                     ShoppingCartId = 3
                 }
                );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Shoe",
                    Price = 34.5M,
                    Images = "main-product01.jpg",
                    Rating = 3,
                    Ammount = 5,
                    Category = Category.Shoes,
                    UserId = "1",
                    Active = true                    
                },
                new Product
                {
                    Id = 2,
                    Name = "Brown Bag",
                    Price = 20.5M,
                    Images = "banner01.jpg",
                    Rating = 4,
                    Category = Category.Accesories,
                    UserId = "2",
                    Active = true,
                    Ammount = 3

                },
                new Product
                {
                    Id = 3,
                    Name = "Wallet",
                    Price = 10.5M,
                    Images = "product03.jpg",
                    Rating = 5,
                    Category = Category.Accesories,
                    UserId = "3",
                    Active = true,
                    Ammount = 2
                },
                new Product
                {
                    Id = 4,
                    Name = "Blue Shoe",
                    Price = 34.5M,
                    Images = "product04.jpg",
                    Rating = 3,
                    Category = Category.Shoes,
                    UserId = "1",
                    Active = true,
                    Ammount = 8
                },
                new Product
                {
                    Id = 5,
                    Name = "Black Heels",
                    Price = 39.5M,
                    Images = "product05.jpg",
                    Rating = 4,
                    Category = Category.Shoes,
                    UserId = "2",
                    Active = true,
                    Ammount = 1

                },
                new Product
                {
                    Id = 6,
                    Name = "Work Bag",
                    Price = 15M,
                    Images = "product06.jpg",
                    Rating = 5,
                    Category = Category.Accesories,
                    UserId = "3",
                    Active = true,
                    Ammount = 4

                });
            modelBuilder.Entity<Review>().HasData(
                 new Review
                 {
                     UserId = "1",
                     ProductId = 2,
                     ReviewText = "Lorem ipsum dolor sit amet, consectetur adipisicing elit," +
                    " sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam," +
                    " quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                     Rating = 3,
                     Date = new DateTime(2019, 12, 26)

                 },
                new Review
                {
                    UserId = "1",
                    ProductId = 3,
                    ReviewText = "Lorem ipsum dolor sit amet, consectetur adipisicing elit," +
                    " sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam," +
                    " quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                    Rating = 4,
                    Date = new DateTime(2019, 12, 13)
                },
                new Review
                {
                    UserId = "2",
                    ProductId = 2,
                    ReviewText = "Lorem ipsum dolor sit amet, consectetur adipisicing elit," +
                    " sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam," +
                    " quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                    Rating = 5,
                    Date = new DateTime(2020, 1, 7)
                },
                new Review
                {
                    UserId = "1",
                    ProductId = 6,
                    ReviewText = "Lorem ipsum dolor sit amet, consectetur adipisicing elit," +
                    " sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam," +
                    " quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                    Rating = 1,
                    Date = new DateTime(2019, 12, 26)
                },
                 new Review
                 {
                     UserId = "2",
                     ProductId = 4,
                     ReviewText = "Lorem ipsum dolor sit amet, consectetur adipisicing elit," +
                    " sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam," +
                    " quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                     Rating = 2,
                     Date = new DateTime(2020, 2, 10)
                 },
                  new Review
                  {
                      UserId = "3",
                      ProductId = 1,
                      ReviewText = "Lorem ipsum dolor sit amet, consectetur adipisicing elit," +
                    " sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam," +
                    " quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                      Rating = 3,
                      Date = new DateTime(2019, 12, 26)
                  }, new Review
                  {
                      UserId = "1",
                      ProductId = 5,
                      ReviewText = "Lorem ipsum dolor sit amet, consectetur adipisicing elit," +
                    " sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam," +
                    " quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                      Rating = 3,
                      Date = new DateTime(2019, 12, 26)
                  }

                );

            
            modelBuilder.Entity<Auction>().HasData(
                new Auction
                {
                    Active = true,
                    ActualPrice = 14,
                    ActualUser = "2",
                    Ammount = 1,
                    Date = new DateTime(2020, 12, 24),
                    Id = 1,
                    InitialPrice = 14,
                    InitialTime = new DateTime(2020, 07, 15),
                    ProductId = 6,
                    User_Sale_ID = "3",
                });

            var shoppifyBankAccount = new BankAccount()
            {
                Id = 1,
                AccountId = "88888888",
                Titular = "Shoppify",
                Ammount = 0.1F
            };

            modelBuilder.Entity<BankAccount>().HasData(shoppifyBankAccount);

            

        }

        
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users{ get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<ProductInCart> ProductInCarts { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts{ get; set; }
        public DbSet<UserInAuction> UsersInAuctions{ get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }

       

    }
}