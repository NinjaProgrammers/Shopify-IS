using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public interface IUserRepository : IRepository<User>
    {
        public User GetByUsername(string name);
        public bool HasAccounts(string Id);
    }
}
