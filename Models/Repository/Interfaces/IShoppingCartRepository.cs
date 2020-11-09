using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public interface IShoppingCartRepository:IRepository<ShoppingCart>
    {
        public ShoppingCart AddEntity(object entity);
    }
}
