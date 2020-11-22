using Microsoft.AspNetCore.Mvc;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Controllers
{
    public class ProductInCartController : Controller
    {
        public readonly IProductInCartRepository repository;
        public ProductInCartController(IProductInCartRepository repository)
        {
            this.repository = repository;
        }

        public ViewResult All()
        {
            var list = repository.GetAll();
            return View(list.ToList());
        }

        public ViewResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(ProductInCart position)
        {
            if (!ModelState.IsValid)
                return View();
            repository.AddEntity(position);
            return RedirectToAction("All");
        }
        public IActionResult Delete(long ProductId, long ShoppingCartId)
        {
            repository.Delete(new Tuple<long,long>(ProductId, ShoppingCartId));
            return RedirectToAction("All");
        }

        [HttpGet]
        public ViewResult Update(long ProductId, long ShoppingCartId)
        {
            var item = repository.GetById(new Tuple<long, long>(ProductId, ShoppingCartId));
            return View(item);
        }
        [HttpPost]
        public IActionResult Update(ProductInCart item)
        {
            repository.Update(item);
            return RedirectToAction("All");
        }
    }
}
