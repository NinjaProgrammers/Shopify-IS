using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class AllNotificationViewModel
    {
        public IEnumerable<Notification> Notifications { get; set; }

        public Dictionary<string,string> Dictionary { get; set; }

        public DateTime Date_Search { get; set; }
        public string Text_Search { get; set; }
        public AllNotificationViewModel(IEnumerable<Notification> notifications)
        {
            Notifications = notifications;
            Dictionary = new Dictionary<string, string>();
            string[] array = { "New user in Auction", "User raise price in Auction", "Auction Completed", "Product saled completly", 
                "Product saled", "Product bought","Error in Auction Sale", "Error in Auction Buy" };
            int count = 0;
            foreach (var item in Enum.GetNames(typeof(NotificationType)))
            {
                Dictionary[item] = array[count];
                ++count;
            }
        }
        public AllNotificationViewModel()
        {

        }
    }

}

