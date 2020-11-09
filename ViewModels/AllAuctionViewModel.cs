using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class AllAuctionViewModel
    {
        public IEnumerable<Auction> Auctions { get; set; }
        public List<SelectListItem> selectListCategory { get; set; }
        public List<SelectListItem> selectListFilter { get; set; }
        public Category Category { get; set; }
        public Filters Filter { get; set; }
        public int FirstPage { get; set; }
        public int Page { get; set; }
    }
}
