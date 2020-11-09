using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class BankAccount
    {
        public long Id { get; set; }
        
        public string AccountId { get; set; }

        public string Titular { get; set; }
        public string UserId { get; set; }

        public float Ammount { get; set; }

        public User User { get; set; }
    }
}
