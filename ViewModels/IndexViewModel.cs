using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class IndexViewModel
    {
        public List<Product> Banner { get; set; }
        public List<Product> Favorites { get; set; }

        //6 products aloud
        public List<Auction> Auction { get; set; }
        public List<Auction> MoneyAuctions { get; set; }

        //Just 3 products aloud
        public Product New { get; set; }


    }
}
