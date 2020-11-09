using Microsoft.AspNetCore.Mvc;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Controllers
{
    public class UserInAuctionController : Controller
    {
        public readonly IUserInAuctionRepository repository;
        public UserInAuctionController(IUserInAuctionRepository repository)
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
        public IActionResult Add(UserInAuction position)
        {
            if (!ModelState.IsValid)
                return View();
            repository.AddEntity(position);
            return RedirectToAction("All");
        }
        public IActionResult Delete(long AuctionId, long UserId)
        {
            repository.Delete(new Tuple<long,long>(AuctionId, UserId));
            return RedirectToAction("All");
        }

        [HttpGet]
        public ViewResult Update(long AuctionId, long UserId)
        {
            var item = repository.GetById(new Tuple<long, long>(AuctionId, UserId));
            return View(item);
        }
        [HttpPost]
        public IActionResult Update(UserInAuction item)
        {
            repository.Update(item);
            return RedirectToAction("All");
        }
    }
}
