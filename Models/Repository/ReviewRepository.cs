using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class ReviewRepository : IReviewRepository
    {
        ApplicationDBContext context;
        public ReviewRepository(ApplicationDBContext context)
        {
            this.context = context;
        }
        public void AddEntity(Review entity)
        {
            context.Reviews.Add(entity);
            context.SaveChanges();
        }

        public void Delete(object Id)
        {
            var id = (Tuple<long, string>)Id;
            var item = context.Reviews.FirstOrDefault(x => x.ProductId == id.Item1 && x.UserId == id.Item2);
            if (item != null)
                context.Reviews.Remove(item);
            context.SaveChanges();
        }

        public bool Exists(string UserId, int ProductId)
        {
            foreach (var item in context.Reviews)
            {
                if (item.ProductId == ProductId && item.UserId == UserId) return true;
            }
            return false;
        }

        public IEnumerable<Review> GetAll()
        {
            return context.Reviews;
        }

        public Review GetById(object Id)
        {
            var id = (Tuple<long, string>)Id;
            return context.Reviews.FirstOrDefault(x => x.ProductId == id.Item1 && x.UserId == id.Item2);
        }

        public IEnumerable<Review> Select(Review entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Review entity)
        {
            var item = context.Reviews.FirstOrDefault(x => x.ProductId == entity.ProductId && x.UserId == entity.UserId);
            if (item != null)
            {
                item.Date = entity.Date;
                item.Product = entity.Product;
                item.ProductId = entity.ProductId;
                item.Rating = entity.Rating;
                item.ReviewText = entity.ReviewText;
                item.User = entity.User;
                item.UserId = entity.UserId;
            }
            context.SaveChanges();
        }
    }
}
