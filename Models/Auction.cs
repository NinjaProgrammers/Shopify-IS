using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Auction : Sale
    {   
       

        [Required(ErrorMessage = "This field is required.")]  
        [DataType(DataType.DateTime,ErrorMessage ="Invalid date")]
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

    }
}
