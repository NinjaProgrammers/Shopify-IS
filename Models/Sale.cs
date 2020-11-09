using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Project.Models
{
    public class Sale
    {
        [Key]
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

        [ForeignKey("User_Buy_ID")]
        public User User_Buy { get; set; }

        [ForeignKey("User_Sale_ID")]
        public User User_Sale { get; set; }

        [ForeignKey("ProductId")]
        public  Product Product { get; set; }

        public bool Active { get; set; }



    }

}
