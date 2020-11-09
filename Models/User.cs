using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Project.Models
{
    public class User : IdentityUser
    {     
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }
        public string City { get; set; }
        public long AccountID { get; set; }

        [StringLength(100)]
        public string Info { get; set; }

        public long ShoppingCartId { get; set; }
        public ShoppingCart Cart { get; set; }

        public List<Review> Reviews { get; set; }

        public List<Sale> Sales { get; set; }
        public List<Sale> Buys { get; set; }

        public List<UserInAuction> UsersInAuctions { get; set; }

        public bool Active { get; set; }

    }
}
