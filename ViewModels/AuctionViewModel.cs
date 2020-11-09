using Project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class AuctionViewModel
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public decimal Import { get; set; }
        [Required]
        public int Ammount { get; set; }
        public string User_Buy_ID { get; set; }
        public string User_Sale_ID { get; set; }
        public User User_Buy { get; set; }
        public User User_Sale { get; set; }
        public Product Product { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date")]
        public DateTime InitialTime { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public DateTime TotalTime { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string ActualUser { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public decimal ActualPrice { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public decimal InitialPrice { get; set; }
        public List<UserInAuction> UsersInAuctions { get; set; }
        public List<Notification> Notifications { get; set; }
        public string  ProductName { get; set; }
        public IEnumerable<Tuple<Review,User>> Review { get; set; }
        public string Rate { get; set; }
        public Review WrittenReview { get; set; }

    }
}
