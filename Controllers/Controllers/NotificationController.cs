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
    public class NotificationController :Controller
    {
        public readonly INotificationRepository repository;
        private readonly IProductRepository productRepository;
        private readonly IUserRepository userRepository;

        public NotificationController(INotificationRepository repository, IProductRepository productRepository, IUserRepository userRepository)
        {
            this.repository = repository;
            this.productRepository = productRepository;
            this.userRepository = userRepository;
        }
        
        public ViewResult All()
        {
            string username = User.Identity.Name;
            User user = userRepository.GetByUsername(username);
            IEnumerable<Notification> list = repository.GetAll().Where(x => x.UserId == user.Id);
            AllNotificationViewModel viewModel = new AllNotificationViewModel(list);
            return View(viewModel);
        }

        [HttpPost]
        public ViewResult All(AllNotificationViewModel viewModel)
        {
            string username = User.Identity.Name;
            User user = userRepository.GetByUsername(username);
            DateTime default_date = new DateTime();
            viewModel.Text_Search = viewModel.Text_Search == null ? "" : viewModel.Text_Search;
            if (viewModel.Date_Search == default_date)
                viewModel.Notifications = repository.GetAll().Where(x => x.UserId == user.Id && (viewModel.Text_Search == "" || x.Text.Contains(viewModel.Text_Search)));
            else
                viewModel.Notifications = repository.GetAll().Where(x => (x.UserId == user.Id && (viewModel.Text_Search == "" || x.Text.Contains(viewModel.Text_Search))) && x.Date == viewModel.Date_Search);
            return View(viewModel);
        }

        public ViewResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Notification position)
        {
            if (!ModelState.IsValid)
                return View();
            repository.AddEntity(position);
            return RedirectToAction("All");
        }

        [HttpPost]
        public IActionResult AddIlicit(IlicitProductViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("IlicitProduct", "Product");

            Notification notification = new Notification()
            {
                Text = $"Your announcement {model.ProductName} has ilicit content on in. It was removed from our site. ",
                UserId = model.UserId
            };
            productRepository.Delete(model.ProductId);
            repository.AddEntity(notification);
            return RedirectToAction("IlicitProduct","Product");
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
        public IActionResult Update(Notification item)
        {
            repository.Update(item);
            return RedirectToAction("All");
        }
    }
}
