using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class UserInAuction
    {
        //FK
        public long AuctionId { get; set; }     
        //FK
        public string UserId { get; set; }

        public Auction Auction { get; set; }
        public User User { get; set; }

        public bool Active { get; set; }

        public DateTime LastActionDate { get; set; }
        public string LastAction { get; set; }
    }
}
