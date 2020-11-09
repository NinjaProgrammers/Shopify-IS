using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Project.Models.NotificationType;

namespace Project.Models
{
    public class Notification
    {
        public Notification()
        {

        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Notification type</param>
        /// <param name="userId">Id of the user to send the notification to</param>
        /// <param name="args">args[0] is the user that bougth, args[1] is the product, args[2] product price</param>
        public Notification(NotificationType type, string userId,params object[] args)
        {
            Date = DateTime.Now;
            UserId = userId;
            switch (type)
            {
                case NewUserInAuction:
                    Text = $"The user {args[0].ToString()} has entered the auction of the product {args[1].ToString()} on {Date}";
                    NotificationType = NewUserInAuction;
                    break;
                case RaisePrice:
                    Text = $"The user {args[0].ToString()} raise the price of the product {args[1].ToString()} to {args[2].ToString()} on {Date}";
                    NotificationType = RaisePrice;
                    break;
                case AuctionCompleted:
                    Text = $"The auction of the product {args[1].ToString()} has finished. The product was sold to {args[0].ToString()} on {args[2].ToString()} on {Date}";
                    NotificationType = AuctionCompleted;
                    break;
                case SaleCompleted:
                    Text = $"The product {args[1].ToString()}  was sold to {args[0].ToString()} on {args[2].ToString()} each of the {args[3].ToString()} pieces giving a " +
                        $"total of {args[4].ToString()} on {Date}. The ammount of the product is now zero.";
                    NotificationType = SaleCompleted;
                    break;
                case SaleProduct:
                    Text = $"The product {args[1].ToString()}  was sold to {args[0].ToString()} on {args[2].ToString()} each of the {args[3].ToString()} pieces giving a " +
                         $"total of {args[4].ToString()} on {Date}";
                    NotificationType = SaleProduct;
                    break;
                case BuyProduct:
                    Text = $"You bought the product {args[1].ToString()}  on {args[2].ToString()} each of the {args[3].ToString()} pieces giving a " +
                              $"total of {args[4].ToString()} on {Date}";
                    NotificationType = BuyProduct;
                    break;
                case BadAuctionSale:
                    Text = $"The sale of your product {args[1].ToString()} available on auction was cancelled due to error on bank account transfer.";
                    NotificationType = BadAuctionSale;
                    break;
                case BadAuctionBuy:
                    Text = $"The buy of the product {args[1].ToString()} bought on auction was cancelled due to error on bank account transfer.";
                    NotificationType = BadAuctionSale;
                    break;
            }
        }

        public int Id { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public NotificationType NotificationType { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public long? AuctionId { get; set; }
    }
}
