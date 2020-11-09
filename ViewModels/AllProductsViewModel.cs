using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class AllProductsViewModel
    {
        public List<Product> Products{ get; set; }
        public Filters Filter { get; set; }
        public List<SelectListItem> selectListItems { get; set; }
        public int Page { get; set; }
        public int FirstPage { get; set; }
        public Category Category { get; set; }
        public List<SelectListItem> selectListCategory { get; set; }

        [Range(0,int.MaxValue,ErrorMessage ="Invalid Ammount.")]
        public int Ammount { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

    }
}
