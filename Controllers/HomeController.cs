using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project.Models;
using Project.Models.Repository.Interfaces;
using Project.ViewModels;

namespace Project.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IProductRepository productRepository;
        private readonly IUserRepository userRepository;
        private readonly IAuctionRepository auctionRepository;

        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository,IUserRepository userRepository, IAuctionRepository auctionRepository)
        {
            this.productRepository = productRepository;
            this.userRepository = userRepository;
            this.auctionRepository = auctionRepository;
            _logger = logger;
        }


        public IActionResult Index()
        {
            IndexViewModel indexViewModel = new IndexViewModel()
            {
                Banner = productRepository.Banner().ToList(),
                Auction = auctionRepository.MostPopularAuction(6).ToList(),
                Favorites = productRepository.Favorites().Take(3).ToList(),
                New = productRepository.New(),
                MoneyAuctions = auctionRepository.MostMoneyAuction(6).ToList()
            };
            foreach (var item in indexViewModel.Auction)
            {
                item.Product = productRepository.GetById(item.ProductId);                
            }
            foreach (var item in indexViewModel.MoneyAuctions)
            {
                item.Product = productRepository.GetById(item.ProductId);
            }
            return View(indexViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ViewResult AboutUs()
        {
            return View();
        }
        
        [HttpGet]
        public ViewResult FAQ()
        {
            return View();
        }       
    }
}
