using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Product
    {

        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }
        
        public string Images { get; set; }
        [Required]
        public Category Category { get; set; }
        [Required]
        public int Ammount { get; set; }
        public int Rating { get; set; }

        //FK
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public List<Review> Reviews { get; set; }
        public List<ProductInCart> ProductInCars { get; set; }

        public bool Active { get; set; }

        public bool IlicitContent { get; set; }

    }

}
