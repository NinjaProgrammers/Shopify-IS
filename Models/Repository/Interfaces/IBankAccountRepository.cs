using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public interface IBankAccountRepository : IRepository<BankAccount>
    {
        public IEnumerable<BankAccount> GetByUserId(string userId);
        public BankAccount GetByTitular(string Titular);
    }
}
