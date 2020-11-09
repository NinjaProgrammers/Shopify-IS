using Microsoft.AspNetCore.Http;
using Project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class ProductCreateViewModel
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public decimal? Price { get; set; }

        public List<IFormFile> Images { get; set; }
        [Required]
        public Category Category { get; set; }
        [Required]
        public int? Ammount { get; set; }
        public int Rating { get; set; }

        public string Username { get; set; }
        public long UserId { get; set; }
        public List<string> img { get; set; }

    }
}
