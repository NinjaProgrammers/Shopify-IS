using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public enum NotificationType
    {
        NewUserInAuction,
        RaisePrice,
        AuctionCompleted,
        SaleCompleted,   
        SaleProduct,
        BuyProduct,
        BadAuctionSale,
        BadAuctionBuy
    }
}
