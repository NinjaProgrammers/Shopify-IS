using Project.Models.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        ApplicationDBContext context;
        public NotificationRepository(ApplicationDBContext context)
        {
            this.context = context;
        }
        public void AddEntity(Notification entity)
        {
            context.Notifications.Add(entity);
            context.SaveChanges();
        }

        public void Delete(object Id)
        {
            var item = context.Notifications.FirstOrDefault(x => x.Id == (long)Id);
            if (item != null)
                context.Notifications.Remove(item);
            context.SaveChanges();
        }

        public IEnumerable<Notification> GetAll()
        {
            return context.Notifications;
        }

        public Notification GetById(object Id)
        {
            return context.Notifications.FirstOrDefault(x => x.Id == (long)Id);
        }

        public IEnumerable<Notification> Select(Notification entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Notification entity)
        {
            var item = context.Notifications.FirstOrDefault(x => x.Id == entity.Id);
            if (item != null)
            {
                item.Text = entity.Text;
                item.User = entity.User;
                item.UserId = entity.UserId;
            }
            context.SaveChanges();
        }

        public IEnumerable<Notification> UserNotifications(string userId)
        {
            return context.Notifications.Where(x => x.UserId == userId);
        }

        public IEnumerable<Notification> AuctionNotification(long auctionId)
        {
             return context.Notifications.Where(x => x.AuctionId == auctionId).OrderByDescending(x => x.Date);
        }
    }
}
