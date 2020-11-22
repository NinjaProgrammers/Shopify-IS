using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project
{
    public static class Bank
    {
        public static bool Deposit(BankAccount UserPay, IEnumerable<BankAccount> UsersToPay, IEnumerable<float> MoneyToPay)
        {
            float total = MoneyToPay.Sum();
            if (UserPay.Ammount < total) return false;
            foreach (var item in UsersToPay.Zip(MoneyToPay))
            {
                UserPay.Ammount -= item.Second;
                item.First.Ammount += item.Second;
            }
            return true;
        }
    }
}
