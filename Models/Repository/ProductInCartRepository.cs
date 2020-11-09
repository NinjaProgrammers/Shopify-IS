using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models.Repository
{
    public class ProductInCartRepository : IProductInCartRepository
    {
        ApplicationDBContext context;
        public ProductInCartRepository(ApplicationDBContext context)
        {
            this.context = context;
        }
        public void AddEntity(ProductInCart entity)
        {
            context.ProductInCarts.Add(entity);
            context.SaveChanges();
        }

        public async Task<int> Delete(object Id)
        {
            var id = (Tuple<long, long>)Id;
            var item = context.ProductInCarts.FirstOrDefault(x => x.ProductId == id.Item1 && x.ShoppingCartId == id.Item2);
            if (item != null)
                context.ProductInCarts.Remove(item);
            return await context.SaveChangesAsync();
        }

        public IEnumerable<ProductInCart> GetAll()
        {
            return context.ProductInCarts;
        }

        public IEnumerable<ProductInCart> GetByCartId(long id)
        {
            foreach (var item in context.ProductInCarts)
            {
                if (item.ShoppingCartId == id) yield return item;
            }
        }

        public ProductInCart GetById(object Id)
        {
            var id = (Tuple<long, long>)Id;
            return context.ProductInCarts.FirstOrDefault(x => x.ProductId == id.Item1 && x.ShoppingCartId == id.Item2);
        }

        public IEnumerable<ProductInCart> Select(ProductInCart entity)
        {
            throw new NotImplementedException();
        }

        public void Update(ProductInCart entity)
        {
            var item = context.ProductInCarts.FirstOrDefault(x => x.ProductId == entity.ProductId && x.ShoppingCartId == entity.ShoppingCartId);
            if (item != null)
            {
                item.Product = entity.Product;
                item.SCart = entity.SCart;
                item.ProductId = entity.ProductId;
                item.ShoppingCartId = entity.ShoppingCartId;
                item.Ammount = entity.Ammount;
            }
            context.Update(item);
            //context.SaveChanges();
        }

        void IRepository<ProductInCart>.Delete(object Id)
        {
            var id = (Tuple<long, long>)Id;
            var item = context.ProductInCarts.FirstOrDefault(x => x.ProductId == id.Item1 && x.ShoppingCartId == id.Item2);
            if (item != null)
                context.ProductInCarts.Remove(item);
            context.SaveChanges();
        }
    }
}
