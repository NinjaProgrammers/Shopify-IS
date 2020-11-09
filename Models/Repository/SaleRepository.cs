using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class SaleRepository : ISaleRepository
    {
        ApplicationDBContext context;
        public SaleRepository(ApplicationDBContext context)
        {
            this.context = context;
        }
        public void AddEntity(Sale entity)
        {
            context.Sales.Add(entity);
            context.SaveChanges();
        }

        public void Delete(object Id)
        {
            var item = context.Sales.FirstOrDefault(x => x.Id == (long)Id);
            if (item != null)
                context.Sales.Remove(item);
            context.SaveChanges();
        }

        public IEnumerable<Sale> GetAll()
        {
            return context.Sales;
        }

        public Sale GetById(object Id)
        {
            return context.Sales.FirstOrDefault(x => x.Id == (long)Id);
        }

        public IEnumerable<Sale> Select(Sale entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Sale entity)
        {
            var item = context.Sales.FirstOrDefault(x => x.Id == entity.Id);
            if (item != null)
            {
                item.Ammount = entity.Ammount;
                item.Date = entity.Date;
                item.Import = entity.Import;
                item.Product = entity.Product;
                item.ProductId = entity.ProductId;
                item.User_Buy = entity.User_Buy;
                item.User_Buy_ID = entity.User_Buy_ID;
                item.User_Sale = entity.User_Sale;
                item.User_Sale_ID = entity.User_Sale_ID;                
            }
            context.SaveChanges();
        }
    }
}
