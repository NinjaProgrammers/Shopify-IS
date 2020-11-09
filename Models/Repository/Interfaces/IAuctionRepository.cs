using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models.Repository.Interfaces
{
    public interface IAuctionRepository : IRepository<Auction>
    {
        public IEnumerable<Auction> MostPopularAuction(int count = 10);
        public Auction GetAuction(long id);
        public IEnumerable<Auction> MostMoneyAuction(int count = 10);
    }
}
