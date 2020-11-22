using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Models;
using Project.Models.Repository.Interfaces;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Controllers
{
    public class AuctionController : Controller
    {
        public readonly IAuctionRepository repository;
        private readonly IProductRepository productRepository;
        private readonly IUserRepository userRepository;
        private readonly IUserInAuctionRepository userInAuctionRepository;
        private readonly INotificationRepository notificationRepository;
        private readonly IBankAccountRepository bankAccountRepository;
        private List<SelectListItem> selectListItemsCategory;
        private List<SelectListItem> selectListItemsFilters;

        public AuctionController(IAuctionRepository repository, IProductRepository productRepository, IUserRepository userRepository, 
            IUserInAuctionRepository userInAuctionRepository, INotificationRepository notificationRepository,IBankAccountRepository bankAccountRepository)
        {
            this.repository = repository;
            this.productRepository = productRepository;
            this.userRepository = userRepository;
            this.userInAuctionRepository = userInAuctionRepository;
            this.notificationRepository = notificationRepository;
            this.bankAccountRepository = bankAccountRepository;
            SelectList();
           
        }

        private void SelectList()
        {
            int count = 0;
            string[] array = { "None", "Price Down", "Price Up", "Rating Down", "Rating Up", "Name Down", "Name Up" };
            selectListItemsFilters = new List<SelectListItem>();
            selectListItemsCategory = new List<SelectListItem>();

            foreach (var item in Enum.GetNames(typeof(Filters)))
            { 
               selectListItemsFilters.Add(new SelectListItem(array[count], item));
                ++count;
            }
            foreach (var item in Enum.GetNames(typeof(Category)))
            {
                selectListItemsCategory.Add(new SelectListItem(item, item));
            }
        }

        [AllowAnonymous]
        public ViewResult All()
        {
            var list = repository.GetAll().Where(x => x.Active);
            var to_update = new List<Auction>();

            List<BankAccount> usersToPay = new List<BankAccount>();
            List<float> moneyToPay = new List<float>();

            foreach (var item in list)
            {              

                item.Product = productRepository.GetById(item.ProductId);
                if (item.Date <= DateTime.Now && item.Active)
                {
                    item.Active = false;
                    if (item.ActualUser == null) continue;
                    to_update.Add(item);

                    //Money Transfer
                    usersToPay.Add(bankAccountRepository.GetByUserId(item.User_Sale_ID).First());
                    moneyToPay.Add(item.Ammount * (float)item.ActualPrice);
                    usersToPay.Add(bankAccountRepository.GetByTitular("Shoppify"));
                    moneyToPay.Add((moneyToPay.Sum() * 2) / 100);
                    var temp = bankAccountRepository.GetByUserId(item.ActualUser);
                    if (temp.Count() == 0) continue;
                    var userpay = bankAccountRepository.GetByUserId(item.ActualUser).First();
                    if (!Bank.Deposit(userpay, usersToPay, moneyToPay))
                    {
                        var productname = productRepository.GetById(item.ProductId).Name;
                        Notification notification = new Notification(NotificationType.BadAuctionSale, item.User_Sale_ID, productname);
                        Notification notification1 = new Notification(NotificationType.BadAuctionBuy, item.ActualUser, productname);
                        notificationRepository.AddEntity(notification);
                        notificationRepository.AddEntity(notification1);
                    }
                    usersToPay.Clear();
                    moneyToPay.Clear();
                }
                
            }

            foreach (var item in to_update)
            {
                repository.Update(item);
            }
            AllAuctionViewModel viewModel = new AllAuctionViewModel()
            {
                Auctions = list,
                FirstPage = 1,
                Page = 1,
                selectListCategory = selectListItemsCategory,
                selectListFilter = selectListItemsFilters
            };
            return View(viewModel);
        }

        [HttpPost]
        public ViewResult All(AllAuctionViewModel viewModel)
        {
             
            var page = viewModel.Page;
            if (page <= 0) page = 1;
            if (page % 3 == 0)
                viewModel.FirstPage = page - 2;
            else
                viewModel.FirstPage = page / 3 > 0 ? page / 3 * 3 + 1 : 1;
            if (viewModel.FirstPage <= 0) viewModel.FirstPage = 1;


            var s = viewModel.Filter;
            var categ = viewModel.Category;
            int count = 0;
            string[] array = { "None", "Price Down", "Price Up", "Rating Down", "Rating Up", "Name Down", "Name Up" };

            viewModel.selectListFilter = new List<SelectListItem>();
            viewModel.selectListCategory = new List<SelectListItem>();

            foreach (var item in Enum.GetNames(typeof(Filters)))
            {
                if (s.ToString() == item)
                {
                    viewModel.selectListFilter.Add(new SelectListItem(array[count], item, true));
                }
                else
                {
                    viewModel.selectListFilter.Add(new SelectListItem(array[count], item));
                }
                ++count;
            }

            foreach (var item in Enum.GetNames(typeof(Category)))
            {
                if (categ.ToString() == item)
                {
                    viewModel.selectListCategory.Add(new SelectListItem(item, item, true));
                }
                else
                {
                    viewModel.selectListCategory.Add(new SelectListItem(item, item));
                }
            }
            switch (s)
            {
                case Filters.None:
                    viewModel.Auctions = repository.GetAll().Skip((page - 1) * 9).Where(x => x.Active & (categ == Category.All
                    | categ == productRepository.GetById(x.ProductId).Category)).ToList();
                    break;
                case Filters.PriceDown:
                    viewModel.Auctions = repository.GetAll().Skip((page - 1) * 9).Where(x => x.Active & (categ == Category.All
                    | categ == productRepository.GetById(x.ProductId).Category)).OrderByDescending(x => x.ActualPrice).ToList();
                    break;
                case Filters.PriceUp:
                    viewModel.Auctions = repository.GetAll().Skip((page - 1) * 9).Where(x => x.Active & (categ == Category.All
                    | categ == productRepository.GetById(x.ProductId).Category)).OrderBy(x => x.ActualPrice).ToList();
                    break;
                case Filters.RatingDown:
                    viewModel.Auctions = repository.GetAll().Skip((page - 1) * 9).Where(x => x.Active & (categ == Category.All
                    | categ == productRepository.GetById(x.ProductId).Category)).OrderByDescending(x => productRepository.GetById(x.ProductId).Rating).ToList();
                    break;
                case Filters.RatingUp:
                    viewModel.Auctions = repository.GetAll().Skip((page - 1) * 9).Where(x => x.Active & (categ == Category.All
                    | categ == productRepository.GetById(x.ProductId).Category)).OrderBy(x => productRepository.GetById(x.ProductId).Rating).ToList();
                    break;
                case Filters.NameDown:
                    viewModel.Auctions = repository.GetAll().Skip((page - 1) * 9).Where(x => x.Active & (categ == Category.All
                    | categ == productRepository.GetById(x.ProductId).Category)).OrderByDescending(x => productRepository.GetById(x.ProductId).Name).ToList();
                    break;
                case Filters.NameUp:
                    viewModel.Auctions = repository.GetAll().Skip((page - 1) * 9).Where(x => x.Active && (categ == Category.All 
                    |  categ == productRepository.GetById(x.ProductId).Category)).OrderBy(x => productRepository.GetById(x.ProductId).Name).ToList();
                    break;
                default:
                    break;
            }
            return View(viewModel);
        }

        [HttpGet]                
        public ViewResult Add(long id)
        {
            Auction auction = new Auction()
            {
                Product = productRepository.GetById(id)
            };
            return View(auction);
        }

        [HttpPost]
        public IActionResult Add(Auction auction)
        {
            auction.Id = 0;
            auction.Product = productRepository.GetById(auction.ProductId);
            if (!ModelState.IsValid)
            {
                
                if (auction.Ammount == 0 || auction.Ammount > auction.Product.Ammount) ViewBag.AmmountError = "The product ammount for the auction is not valid.";
                if (auction.InitialPrice.Equals(0) ) ViewBag.PriceError = "The product initial price is not valid.";                
                return View(auction);
            }

            if (auction.Ammount > auction.Product.Ammount)
            {
                ViewBag.AmmountError = "The product ammount for the auction is not valid.";
                return View(auction);
            }
            
            auction.InitialTime = DateTime.Now;
            auction.User_Sale = userRepository.GetByUsername(User.Identity.Name);
            auction.ProductId = auction.Product.Id;
            auction.ActualPrice = auction.InitialPrice;
            auction.Active = true;
            repository.AddEntity(auction);
            return RedirectToAction("All");
        }
        public IActionResult Delete(long Id)
        {
            repository.Delete(Id);
            return RedirectToAction("All");
        }

        [HttpGet]
        public ViewResult Update(long Id)
        {
            var item = repository.GetById(Id);
            return View(item);
        }
        [HttpPost]
        public IActionResult Update(Auction item)
        {
            repository.Update(item);
            return RedirectToAction("All");
        }

        public IActionResult Auction(long id)
        {
            User user = userRepository.GetByUsername(User.Identity.Name);
            Auction auction = repository.GetAuction(id);

            ViewBag.BidError = TempData["BidError"];
            ViewBag.ReviewError = TempData["ReviewError"];
            if (user.Id != auction.User_Sale_ID && userInAuctionRepository.GetById(new Tuple<long,string>(auction.Id,user.Id)) == null )
            {
                Notification notification = new Notification(NotificationType.NewUserInAuction, auction.User_Sale_ID, user.Name, auction.Product.Name, auction.Product.Price);
                notification.AuctionId = auction.Id;
                notificationRepository.AddEntity(notification);
                userInAuctionRepository.AddEntity(new UserInAuction() {
                    AuctionId = auction.Id, 
                    UserId = user.Id,
                    LastActionDate = DateTime.Now,
                    LastAction = "Join the auction"
                });
            }

            AuctionViewModel viewModel = new AuctionViewModel()
            {
                ActualPrice = auction.ActualPrice,
                ActualUser = auction.ActualUser,
                Ammount = auction.Ammount,
                Date = auction.Date,
                Import = auction.ActualPrice * auction.Ammount,
                InitialPrice = auction.InitialPrice,
                InitialTime = auction.InitialTime,
                Product = auction.Product,
                ProductId = auction.ProductId,
                User_Buy = auction.User_Buy,
                User_Buy_ID = auction.User_Buy_ID,
                User_Sale = auction.User_Sale,
                User_Sale_ID = auction.User_Sale_ID,
                Id = auction.Id,
                Notifications = notificationRepository.AuctionNotification(auction.Id).ToList(),
                Review = productRepository.Reviews(auction.ProductId)
            };
            foreach (var item in auction.UsersInAuctions)
            {
               item.User = userRepository.GetById(item.UserId);
            }
            viewModel.UsersInAuctions = auction.UsersInAuctions;           
            viewModel.UsersInAuctions = viewModel.UsersInAuctions.OrderByDescending(x => x.LastActionDate).ToList();
            return View(viewModel);
        }

        public IActionResult Bid(AuctionViewModel viewModel)
        {
            var user = userRepository.GetByUsername(User.Identity.Name);
            if (user.Id == viewModel.User_Sale_ID)
            {
                TempData["BidError"] = "Error! You can not bid your own product";
                return RedirectToAction("Auction", viewModel);
            }

            Auction auction = repository.GetById(viewModel.Id);
            if(viewModel.ActualPrice > auction.ActualPrice)
            {
                Notification notification = new Notification(NotificationType.RaisePrice, viewModel.User_Sale_ID, user.UserName, viewModel.ProductName, viewModel.ActualPrice);
                notification.AuctionId = auction.Id;
                notificationRepository.AddEntity(notification);
                auction.ActualPrice = viewModel.ActualPrice;
                auction.ActualUser = user.Id;
                repository.Update(auction);
                var userInAuction = userInAuctionRepository.GetById(new Tuple<long, string>(auction.Id, auction.ActualUser));
                userInAuction.LastActionDate = DateTime.Now;
                userInAuction.LastAction = $"Raise the price to {viewModel.ActualPrice}";
                userInAuctionRepository.Update(userInAuction);
                return RedirectToAction("Auction", viewModel);
            }           
            TempData["BidError"] = "Invalid ammount.";
            return RedirectToAction("Auction", viewModel);
        }

    }
}
