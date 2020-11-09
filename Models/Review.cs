using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Review
    {
        public string UserId { get; set; }
        public long ProductId { get; set; }
        public string ReviewText { get; set; }
        public DateTime Date { get; set; }
        public int Rating { get; set; }

        public User User { get; set; }
        public Product Product{ get; set; }

        public bool Active { get; set; }
    }
}
