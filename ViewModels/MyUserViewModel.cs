using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class MyUserViewModel
    {
        public User User { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<BankAccount> BankAccounts { get; set; }
        public IEnumerable<Notification> Notifications { get; set; }
    }
}
