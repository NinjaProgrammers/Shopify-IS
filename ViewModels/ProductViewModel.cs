using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public Review Review { get; set; }
        public List<Tuple<Review,User>> Reviews { get; set; }
        public List<Product> Favorites { get; set; }

        public string  Username { get; set; }

        public string Rate { get; set; }
        public IEnumerable<string> Files { get; set; }
        public int Ammount { get; set; }
        public long ProductId { get; set; }

        public int ReviewPage { get; set; }
        public int ReviewFirstPage { get; set; }
    }
}
