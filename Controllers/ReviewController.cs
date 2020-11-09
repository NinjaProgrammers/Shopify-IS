using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Controllers
{
    public class ReviewController : Controller
    {
        public readonly IReviewRepository repository;
        private readonly IUserRepository userRepository;
        private readonly IProductRepository productRepository;

        public ReviewController(IReviewRepository repository, IUserRepository userRepository, IProductRepository productRepository)
        {
            this.repository = repository;
            this.userRepository = userRepository;
            this.productRepository = productRepository;
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
        public IActionResult Add(ProductViewModel model)
        {
            string id = userRepository.GetByUsername(model.Username).Id;
            var Id = model.Review.ProductId;
            if (!ModelState.IsValid || repository.Exists(id, (int)Id))
            {
                TempData["ReviewError"] = "You have already inserted a review on this product.";
                return RedirectToAction("Product", "Product", new { Id });
            }

            var productowner = productRepository.GetById(Id).UserId;
            if(productowner == id)
            {
                TempData["ReviewError"] = "You can not add a review on your own product.";
                return RedirectToAction("Product", "Product", new { Id });
            }
            if (model.Rate == null)
            {
                TempData["ReviewError"] = "You must select a star value";
                return RedirectToAction("Auction", "Auction", new { Id });
            }

            Review r = new Review()
            {
                Date = DateTime.Now,
                ProductId = Id,
                Rating = int.Parse(model.Rate),
                ReviewText = model.Review.ReviewText,
                UserId = id
            };

            repository.AddEntity(r);
            return RedirectToAction("Product", "Product",new { Id });
        }

        public IActionResult AddReviewInAuction(AuctionViewModel model)
        {
            string id = userRepository.GetByUsername(User.Identity.Name).Id;
            var Id = model.WrittenReview.ProductId;
            if (!ModelState.IsValid || repository.Exists(id, (int)Id))
            {
                TempData["ReviewError"] = "You have already inserted a review on this product.";
                return RedirectToAction("Auction", "Auction", new { model.Id });
            }

            var productowner = productRepository.GetById(Id).UserId;
            if (productowner == id)
            {
                TempData["ReviewError"] = "You can not add a review on your own product.";
                return RedirectToAction("Auction", "Auction", new { model.Id });
            }
            if(model.Rate == null)
            {
                TempData["ReviewError"] = "You must select a star value";
                return RedirectToAction("Auction", "Auction", new { model.Id });
            }

            Review r = new Review()
            {
                Date = DateTime.Now,
                ProductId = Id,
                Rating = int.Parse(model.Rate),
                ReviewText = model.WrittenReview.ReviewText,
                UserId = id
            };

            repository.AddEntity(r);
            return RedirectToAction("Auction", "Auction", new { model.Id });
        }
        public IActionResult Delete(long ProductId, long UserId)
        {
            repository.Delete(new Tuple<long,long>(ProductId, UserId));
            return RedirectToAction("All");
        }

        [HttpGet]
        public ViewResult Update(long ProductId, long UserId)
        {
            var item = repository.GetById(new Tuple<long, long>(ProductId, UserId));
            return View(item);
        }
        [HttpPost]
        public IActionResult Update(Review item)
        {
            repository.Update(item);
            return RedirectToAction("All");
        }
    }
}
