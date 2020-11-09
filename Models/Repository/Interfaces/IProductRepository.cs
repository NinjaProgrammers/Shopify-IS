using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public interface IProductRepository: IRepository<Product>
    {
        public IEnumerable<Product> Favorites();
        public IEnumerable<Product> Banner();
        public IEnumerable<Product> Auction();

        // El tiempo que queda a cada uno de los productos de las subastas
        public IEnumerable<(int, int, int)> TimeLeft();
        public Product New();
        public long MaxID();
        public IEnumerable<Tuple<Review, User>> Reviews(long Id);

        public IEnumerable<Product> GetUserProducts(string id);
        public IEnumerable<Product> IlicitProducts();

    }
}
