using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        ApplicationDBContext context;
        public ShoppingCartRepository(ApplicationDBContext context)
        {
            this.context = context;
        }
        public ShoppingCart AddEntity(ShoppingCart entity)
        {
            var s = context.ShoppingCarts.Add(entity);
            context.SaveChanges();
            return s.Entity;
        }

        public ShoppingCart AddEntity(object entity)
        {
            var s = context.ShoppingCarts.Add((ShoppingCart)entity);
            context.SaveChanges();
            return s.Entity;
        }

        //public void AddToUser(string id)
        //{
        //    var UserId = context.Users.First(x => x.Id == id).Id;
        //    foreach (var item in context.ShoppingCarts)
        //    {
        //        if (item.UserId == UserId) return;
        //    }
        //    ShoppingCart cart = new ShoppingCart()
        //    {
        //        UserId = UserId
        //    };
        //    AddEntity(cart);
        //}

        public void Delete(object Id)
        {
            var item = context.ShoppingCarts.FirstOrDefault(x => x.Id == (long)Id);
            if (item != null)
                context.ShoppingCarts.Remove(item);
            context.SaveChanges();
        }

        public IEnumerable<ShoppingCart> GetAll()
        {
            return context.ShoppingCarts;
        }

        public ShoppingCart GetById(object Id)
        {
            return context.ShoppingCarts.FirstOrDefault(x => x.Id == (long)Id);
        }

        public IEnumerable<ShoppingCart> Select(ShoppingCart entity)
        {
            throw new NotImplementedException();
        }

        public void Update(ShoppingCart entity)
        {
            var item = context.ShoppingCarts.FirstOrDefault(x => x.Id == entity.Id);
            if (item != null)
            {
                item.ProductInCars = entity.ProductInCars;
            }
            context.SaveChanges();
        }

        void IRepository<ShoppingCart>.AddEntity(ShoppingCart entity)
        {
            context.ShoppingCarts.Add(entity);
            context.SaveChanges();
        }
    }
}
