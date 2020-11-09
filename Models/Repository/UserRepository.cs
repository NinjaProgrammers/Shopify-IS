using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class UserRepository: IUserRepository
    {
        ApplicationDBContext context;
        public UserRepository(ApplicationDBContext context)
        {
            this.context = context;
        }
        public void AddEntity(User entity)
        {
            context.Users.Add(entity);
            context.SaveChanges();
        }

        public void Delete(object Id)
        {
            var item = context.Users.FirstOrDefault(x => x.Id == (string)Id);
            if (item != null)
            {
                foreach (var p in context.Products)                
                    if(p.UserId == (string)Id)
                        p.Active = false;                                    
                item.Active = false;
            }
            context.SaveChanges();
        }

        public IEnumerable<User> GetAll()
        {
            return context.Users;
        }

        public User GetById(object Id)
        {
            return context.Users.FirstOrDefault(x => x.Id == Id.ToString());
        }

        public User GetByUsername(string name)
        {
            return context.Users.FirstOrDefault(x => x.UserName == name);
        }

        public bool HasAccounts(string Id)
        {
            foreach (var item in context.BankAccounts)
            {
                if (item.UserId == Id) return true;
            }
            return false;
        }

        public IEnumerable<User> Select(User entity)
        {
            throw new NotImplementedException();
        }

        public void Update(User entity)
        {
            var item = context.Users.FirstOrDefault(x => x.Id == entity.Id);
            if (item != null)
            {
                item.AccountID = entity.AccountID;
                item.Buys = entity.Buys;
                item.Cart = entity.Cart;
                item.Info = entity.Info;
                item.LastName = entity.LastName;
                item.Name = entity.Name;
                item.Reviews = entity.Reviews;
                item.Sales = entity.Sales;
                item.UserName = entity.UserName;
                item.UsersInAuctions = entity.UsersInAuctions;
                item.City = entity.City;
                item.PhoneNumber = entity.PhoneNumber;
            }
            context.Update(item);
            context.SaveChanges();
        }
    }
}
