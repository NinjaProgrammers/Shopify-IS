using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class CartViewModel
    {
            public int items { get; set; }
            public float total { get; set; }
            public List<Product> products { get; set; }
    }
}
