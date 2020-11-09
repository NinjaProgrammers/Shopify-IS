using Project.Models.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models.Repository
{
    public class AuctionRepository : IAuctionRepository
    {
        ApplicationDBContext context;
        public AuctionRepository(ApplicationDBContext context)
        {
            this.context = context;
        }
        public void AddEntity(Auction entity)
        {
            context.Auctions.Add(entity);
            context.SaveChanges();
        }

        public void Delete(object Id)
        {
            var item = context.Auctions.FirstOrDefault(x => x.Id == (long)Id);            
            if (item != null)
                context.Auctions.Remove(item);
            context.SaveChanges();
        }

        public IEnumerable<Auction> GetAll()
        {
            return context.Auctions;
        }

        public Auction GetById(object Id)
        {
            return context.Auctions.FirstOrDefault(x => x.Id == (long)Id);
        }

        public IEnumerable<Auction> Select(Auction entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Auction entity)
        {
            var item = context.Auctions.FirstOrDefault(x => x.Id == entity.Id);
            if (item != null)
            {
                item.ActualPrice = entity.ActualPrice;
                item.ActualUser = entity.ActualUser;
                item.Ammount = entity.Ammount;
                item.Date = entity.Date;
                item.Import = entity.Import;
                item.InitialPrice = entity.InitialPrice;
                item.InitialTime = entity.InitialTime;
                item.Product = entity.Product;
                item.ProductId = entity.ProductId;
                item.TotalTime = entity.TotalTime;
                item.UsersInAuctions = entity.UsersInAuctions;
                item.User_Buy = entity.User_Buy;
                item.User_Buy_ID = entity.User_Buy_ID;
                item.User_Sale = entity.User_Sale;
                item.User_Sale_ID = entity.User_Sale_ID;
                item.Active = entity.Active;
            }
            context.SaveChanges();
        }

        public Auction GetAuction(long id)
        {
            Auction auction = context.Auctions.Find(id);
            auction.Product = context.Products.Find(auction.ProductId);
            auction.User_Sale = context.Users.Find(auction.User_Sale_ID);
            auction.User_Buy = context.Users.Find(auction.User_Buy_ID);
            auction.ActualUser = auction.User_Buy_ID;
            auction.UsersInAuctions = new List<UserInAuction>();
            foreach (var item in context.UsersInAuctions)
            {
               
            }
            return auction;
        }

        public IEnumerable<Auction> MostPopularAuction(int count = 10)
        {
            var userInauction = context.UsersInAuctions.Take(count).ToList();
            Dictionary<long, int> auct_cant = new Dictionary<long, int>();

            foreach (var item in userInauction)
            {
                if (auct_cant.ContainsKey(item.AuctionId))
                    auct_cant[item.AuctionId] += 1;
                else
                    auct_cant.Add(item.AuctionId, 1);
            }

            List<Tuple<int, long>> a = new List<Tuple<int, long>>();

            foreach (var item in auct_cant)
            {                
                a.Add(new Tuple<int, long>(item.Value, item.Key));
            }
            a.Sort();
            a.Reverse();


            for (int i = 0; i < count && i < a.Count; i++)
            {
                var ret = context.Auctions.First(x => x.Id == a[i].Item2);
                if (ret.Date > DateTime.Now)
                {
                    yield return ret;
                }
            }
        }

        public IEnumerable<Auction> MostMoneyAuction(int count = 10)
        {
            var auctions = context.Auctions;
            List<(decimal, int, Auction)> list = new List<(decimal, int, Auction)>();
            int temp = 0;
            foreach (var item in auctions)
            {
                if (!item.Active) continue;
                if (item.Date < DateTime.Now)
                    continue;
                if (temp == count) break;
                list.Add((item.ActualPrice - item.InitialPrice,temp, item));
                ++temp;
            }
            List<Auction> ret = new List<Auction>();
            list.Sort();
            list.Reverse();
            foreach (var item in list)
            {
                ret.Add(item.Item3);
            }
            return ret;
        }
    }
}
