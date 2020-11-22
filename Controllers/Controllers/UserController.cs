using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Models.Repository.Interfaces;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Controllers
{
    public class UserController : Controller
    {
        public readonly IUserRepository repository;
        private readonly IProductRepository productRepository;
        private readonly IBankAccountRepository bankAccountRepository;
        private readonly INotificationRepository notificationRepository;

        public UserController(IUserRepository repository, IProductRepository productRepository, IBankAccountRepository bankAccountRepository,
            INotificationRepository notificationRepository)
        {
            this.repository = repository;
            this.productRepository = productRepository;
            this.bankAccountRepository = bankAccountRepository;
            this.notificationRepository = notificationRepository;
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
        public IActionResult Add(User position)
        {
            if (!ModelState.IsValid)
                return View();
            repository.AddEntity(position);
            return RedirectToAction("All");
        }
        public IActionResult Delete(long Id)
        {
            repository.Delete(Id);
            return RedirectToAction("All");
        }

        [HttpGet]
        public ViewResult Update(string Id)
        {            
            var item = repository.GetById(Id);
            RegisterViewModel viewModel = new RegisterViewModel()
            {
                Email = item.Email,
                Info = item.Info,
                LastName = item.LastName,
                Name = item.Name,
                UserName = item.UserName,
                Telephone = item.PhoneNumber,
                City = item.City
            };
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Update(RegisterViewModel item)
        {
            User user = new User()
            {
                Id = repository.GetByUsername(User.Identity.Name).Id,
                Name = item.Name,
                Email = item.Email,
                City = item.City,
                Info = item.Info,
                LastName = item.LastName,
                UserName = item.UserName,
                PhoneNumber = item.Telephone
            };
            repository.Update(user);
            return RedirectToAction("MyUser");
        }

        [HttpGet]
        public IActionResult MyUser()
        {
            User user = repository.GetByUsername(User.Identity.Name);
            var products = productRepository.GetUserProducts(user.Id);
            var account = bankAccountRepository.GetByUserId(user.Id);
            var notifications = notificationRepository.UserNotifications(user.Id).OrderByDescending(x => x.Date).Take(10);
            MyUserViewModel viewModel = new MyUserViewModel()
            {
                User = user,
                Products = products,
                BankAccounts = account,
                Notifications = notifications
            };
            return View(viewModel);
        }
    }
}
