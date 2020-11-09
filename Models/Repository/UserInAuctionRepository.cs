using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models.Repository
{
    public class UserInAuctionRepository : IUserInAuctionRepository
    {
        ApplicationDBContext context;
        public UserInAuctionRepository(ApplicationDBContext context)
        {
            this.context = context;
        }
        public void AddEntity(UserInAuction entity)
        {
            context.UsersInAuctions.Add(entity);
            context.SaveChanges();
        }

        public void Delete(object Id)
        {
            var id = (Tuple<long, string>)Id;
            var item = context.UsersInAuctions.FirstOrDefault(x => x.AuctionId == id.Item1 && x.UserId == id.Item2);
            if (item != null)
                context.UsersInAuctions.Remove(item);
            context.SaveChanges();
        }

        public IEnumerable<UserInAuction> GetAll()
        {
            return context.UsersInAuctions;
        }

        public UserInAuction GetById(object Id)
        {
            var id = (Tuple<long, string>)Id;
            return context.UsersInAuctions.FirstOrDefault(x => x.AuctionId == id.Item1 && x.UserId == id.Item2);
        }

        public IEnumerable<UserInAuction> Select(UserInAuction entity)
        {
            throw new NotImplementedException();
        }

        public void Update(UserInAuction entity)
        {
            var item = context.UsersInAuctions.FirstOrDefault(x => x.AuctionId == entity.AuctionId && x.UserId == entity.UserId);
            if (item != null)
            {
                item.Auction = entity.Auction;
                item.AuctionId = entity.AuctionId;
                item.User = entity.User;
                item.UserId = entity.UserId;
            }
            context.SaveChanges();
        }


    }
}
