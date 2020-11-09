using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models.Repository.Interfaces
{
    public interface INotificationRepository : IRepository<Notification>
    {
        public IEnumerable<Notification> UserNotifications(string userId);
        public IEnumerable<Notification> AuctionNotification(long auctionId);

    }
}
