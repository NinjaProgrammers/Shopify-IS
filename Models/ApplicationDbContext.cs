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