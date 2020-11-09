using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class IlicitProductViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
    }
}
