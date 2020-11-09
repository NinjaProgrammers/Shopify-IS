using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Project.Models
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private ApplicationDBContext context;
        List<BankAccount> accounts;

        public BankAccountRepository(ApplicationDBContext con)
        {
            context = con;
            accounts = context.BankAccounts.ToList();                
        }

        public void AddEntity(BankAccount entity)
        {
            Random r = new Random();
            entity.Ammount = r.Next(10, 200);
            context.BankAccounts.Add(entity);
            context.SaveChanges();
        }

        public void Delete(object Id)
        {
            var item = context.Notifications.FirstOrDefault(x => x.Id == (long)Id);
            if (item != null)
                context.Notifications.Remove(item);
            context.SaveChanges();
        }

        public IEnumerable<BankAccount> GetAll()
        {
            return context.BankAccounts;
        }

        public BankAccount GetById(object Id)
        {
            return context.BankAccounts.FirstOrDefault(x => x.Id == (long)Id);
        }

        public IEnumerable<BankAccount> GetByUserId(string userId)
        {
            foreach (var item in context.BankAccounts)
            {
                if (item.UserId == userId) yield return item;
            }
        }

        public BankAccount GetByTitular(string Titular)
        {
            return context.BankAccounts.First(x => x.Titular == Titular);
        }

        public IEnumerable<BankAccount> Select(BankAccount entity)
        {
            throw new NotImplementedException();
        }

        public void Update(BankAccount entity)
        {
            var item = context.BankAccounts.FirstOrDefault(x => x.Id == entity.Id);
            if (item != null)
            {
                item.AccountId = entity.AccountId;
                item.UserId = entity.UserId;
            }
            context.SaveChanges();
        }

    }
}
