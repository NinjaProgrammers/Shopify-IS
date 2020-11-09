using Microsoft.AspNetCore.Mvc;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class AccountAndCartViewModel : ViewComponent
    {
        private readonly IProductInCartRepository repository;
        private readonly IUserRepository userRepository;
        private readonly IProductRepository productRepository;

        public AccountAndCartViewModel(IProductInCartRepository repository, IUserRepository userRepository, IProductRepository productRepository)
        {
            this.repository = repository;
            this.userRepository = userRepository;
            this.productRepository = productRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {

                CartViewModel a = new CartViewModel();
                a.products = new List<Product>();
                return View("_AccountandCart", a);
            }
            User user = userRepository.GetByUsername(User.Identity.Name);
            var products = repository.GetByCartId(user.ShoppingCartId);

            CartViewModel viewModel = new CartViewModel();
            viewModel.products = new List<Product>();

            foreach (var item in products)
            {
                Product product = productRepository.GetById(item.ProductId);
                if (!product.Active) continue;
                Product p = new Product
                {
                    Ammount = item.Ammount,
                    Price = product.Price,
                    Name = product.Name,
                    Images = product.Images,
                    Id = product.Id
                };
                viewModel.products.Add(p);
                viewModel.items += item.Ammount ;
                viewModel.total = (float)product.Price + viewModel.total;
            }

            return View("_AccountandCart", viewModel);



        }
    }
}
