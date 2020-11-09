using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class ProductInCart
    {
        public long ShoppingCartId { get; set; }

        public long ProductId { get; set; }

        public Product Product { get; set; }

        public  ShoppingCart SCart { get; set; }

        public bool Active { get; set; }

        public int Ammount { get; set; }
    }
}
