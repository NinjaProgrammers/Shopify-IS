using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class ShoppingCart
    {
        public long Id { get; set; }

        public List<ProductInCart> ProductInCars { get; set; }
    }
}
