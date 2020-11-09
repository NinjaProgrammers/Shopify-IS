using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class CheckoutViewModel
    {
        public List<Product> Products { get; set; }
        
        public IEnumerable<BankAccount> BankAccounts { get; set; }

        public List<SelectListItem> ListItems { get; set; }
        public long SelectedAccountId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "This field is required")]
        public string Titular { get; set; }

        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "This field is required")]
        public string Telephone { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "This field is required")]
        public string City { get; set; }
        public string BankAccount { get; set; }

    }
}
