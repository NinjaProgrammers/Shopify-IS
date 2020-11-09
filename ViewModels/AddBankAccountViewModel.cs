using Project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class AddBankAccountViewModel
    {
        public long Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "This field is required")]
        public string AccountId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "This field is required")]
        public string Titular { get; set; }

        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "This field is required")]
        public string  Telephone { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "This field is required")]
        public string City { get; set; }

        public User User { get; set; }
    }
}
