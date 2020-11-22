using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Controllers
{
    public class BankAccountController : Controller
    {
        public readonly IBankAccountRepository repository;
        private readonly IUserRepository userRepository;

        public BankAccountController(IBankAccountRepository repository, IUserRepository userRepository)
        {
            this.repository = repository;
            this.userRepository = userRepository;
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
        public IActionResult Add(AddBankAccountViewModel accountViewModel)
        {
            if (!ModelState.IsValid)
                return View();

            User user = userRepository.GetByUsername(User.Identity.Name);

            BankAccount bankAccount = new BankAccount()
            {
                AccountId = accountViewModel.AccountId,
                Titular = accountViewModel.Titular,
                UserId = user.Id
            };
            repository.AddEntity(bankAccount);

            User newUser = new User()
            {
                AccountID = bankAccount.Id,
                Active = true,
                Name = user.Name,
                City = accountViewModel.City,
                Email = user.Email,
                Id = user.Id,
                Info = user.Info,
                LastName = user.LastName,
                PhoneNumber = accountViewModel.Telephone,
                UserName = user.UserName
            };
            userRepository.Update(newUser);

            return RedirectToAction("Index","Home");
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
        public IActionResult Update(BankAccount item)
        {
            repository.Update(item);
            return RedirectToAction("All");
        }


        [HttpGet]
        public IActionResult AddNewAccount()
        {           
            return View();
        }

        [HttpPost]
        public IActionResult AddNewAccount(AddNewBankAccountViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            User user = userRepository.GetByUsername(User.Identity.Name);
            BankAccount bankAccount = new BankAccount()
            {
                AccountId = viewModel.AccountId,
                Titular = viewModel.Titular,
                UserId = user.Id
            };
            repository.AddEntity(bankAccount);
            return RedirectToAction("MyUser", "User");
        }

    }
}
