using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Project.Models
{
    public class ProductRepository : IProductRepository
    {
        private ApplicationDBContext context;
        public ProductRepository(ApplicationDBContext context)
        {
            this.context = context;
        }
        public void AddEntity(Product entity)
        {
            entity.IlicitContent = false;
            context.Products.Add(entity);
            context.SaveChanges();
        }

        public IEnumerable<Product> GetAll()
        {
            return context.Products.Where(x => x.Active);
        }

        public Product GetById(object Id)
        {
            return context.Products.FirstOrDefault(x => x.Id.ToString() == Id.ToString());
        }

        public void Delete(object Id)
        {
            Product p = context.Products.FirstOrDefault(x => x.Id.ToString() == Id.ToString());
            if (p != null)
            {
                p.Active = false;
                context.Update(p);
                context.SaveChanges();
            }
        }

        public void Update(Product entity)
        {
            var item=context.Products.FirstOrDefault(x => x.Id == entity.Id);
            if(item != null)
            {
                item.Ammount = entity.Ammount;
                item.Category = entity.Category;
                item.Description = entity.Description;
                item.Images = entity.Images;
                item.Name = entity.Name;
                item.Price = entity.Price;
                item.ProductInCars = entity.ProductInCars;
                item.Rating = entity.Rating;
                item.Reviews = entity.Reviews;
                item.User = entity.User;
                item.UserId = entity.UserId;
                item.Active = true;
            }
            context.Update(item);
            context.SaveChanges();
        }

        public IEnumerable<Product> Select(Product entity)
        {
            return GetAll();
        }

        public IEnumerable<Product> Favorites()
        {
            return GetAll();
        }

        public IEnumerable<Product> Banner()
        {
            return GetAll();
        }

        public IEnumerable<Product> Auction()
        {
            foreach (var item in context.Auctions)
            {
                yield return context.Products.FirstOrDefault(x => x.Id == item.ProductId);
            }
        }

        public IEnumerable<(int, int, int)> TimeLeft()
        {
            int sz = GetAll().Count();
            for (int i = 0; i < sz; i++)
                yield return(3, 3, 3);
        }

        public Product New()
        {
            return GetAll().Where(x => x.Active).FirstOrDefault();
        }

        public long MaxID()
        {
            return context.Products.Max(x => x.Id);
        }
        public IEnumerable<Tuple<Review,User>> Reviews(long Id)
        {
            foreach (var review in context.Reviews.Where(x => x.ProductId == Id))
                yield return new Tuple<Review, User>(review, context.Users.FirstOrDefault(x => x.Id == review.UserId));
        }

        public IEnumerable<Product> GetUserProducts(string id)
        {
            List<Product> ret = new List<Product>();
            foreach (var item in context.Products.Where(x => x.Active))
            {
                if (item.UserId == id) ret.Add(item);
            }
            return ret;
        }

        public IEnumerable<Product> IlicitProducts()
        {
            List<Product> ret = new List<Product>();
            foreach (var item in context.Products.Where(x => x.Active))
            {
                if (item.IlicitContent) ret.Add(item);
            }
            return ret;
        }

        
    }
}
