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
    public class ShoppingCartController : Controller
    {
        public readonly IShoppingCartRepository repository;
        private readonly IUserRepository userRepository;
        private readonly IProductInCartRepository productInCartRepository;
        private readonly IProductRepository productRepository;
        private readonly IBankAccountRepository bankAccountRepository;
        private readonly INotificationRepository notificationRepository;
        private readonly ISaleRepository saleRepository;
        private User UserId;

        public ShoppingCartController(IShoppingCartRepository repository, IUserRepository userRepository, IProductInCartRepository productInCartRepository,
            IProductRepository productRepository, IBankAccountRepository bankAccountRepository, INotificationRepository notificationRepository,
            ISaleRepository saleRepository)
        {
            this.repository = repository;
            this.userRepository = userRepository;
            this.productInCartRepository = productInCartRepository;
            this.productRepository = productRepository;
            this.bankAccountRepository = bankAccountRepository;
            this.notificationRepository = notificationRepository;
            this.saleRepository = saleRepository;
        }

        public void AddCarts()
        {
            for (int i = 0; i < 5; i++)
            {
                ShoppingCart cart = new ShoppingCart();
                repository.AddEntity(cart);
            }
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
        public IActionResult Add(ShoppingCart position)
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
        public ViewResult Update(long Id)
        {
            var item = repository.GetById(Id);
            return View(item);
        }
        [HttpPost]
        public IActionResult Update(ShoppingCart item)
        {
            repository.Update(item);
            return RedirectToAction("All");
        }

        [HttpGet]
        public IActionResult AddProductById(int id)
        {
            Product product = productRepository.GetById(id);
            var user = userRepository.GetByUsername(User.Identity.Name);
            var cart = repository.GetById(user.ShoppingCartId);
            ProductInCart productInCart = new ProductInCart()
            {
                Ammount = 1,
                ProductId = product.Id,
                ShoppingCartId = cart.Id
            };

            var p = productInCartRepository.GetById(new Tuple<long, long>(product.Id, cart.Id));
            if (p != null)
            {
                p.Ammount++;
                productInCartRepository.Update(p);
            }
            else
            {
                productInCartRepository.AddEntity(productInCart);
            }
            return RedirectToAction("Index", "Home");

        }
                                      

        [HttpPost]
        public IActionResult AddProduct(ProductViewModel entity)
        {
            UserId = userRepository.GetByUsername(User.Identity.Name);
            var cart = repository.GetById(UserId.ShoppingCartId);
            Product p = productRepository.GetById(entity.Product.Id);

            if(p.Ammount< entity.Product.Ammount)
            {
                TempData["AmmountError"]= "The ammount you requested is superior to the existing ammount of the product.";
                return RedirectToAction("Product", "Product", new { id = p.Id });
            }
            if (entity.Product.Ammount <= 0)
            {
                TempData["AmmountError"] = "The ammount you request must be bigger than zero.";
                return RedirectToAction("Product", "Product", new { id = p.Id });
            }
            ProductInCart product = new ProductInCart()
            {
                Ammount = entity.Product.Ammount,
                ProductId = entity.Product.Id,
                ShoppingCartId = cart.Id
            };
            var temp = productInCartRepository.GetById(new Tuple<long, long>(product.ProductId, product.ShoppingCartId));
            if(temp != null)
            {
                if(temp.Ammount + product.Ammount > p.Ammount)
                {
                    TempData["AmmountError"] = "The ammount in your shopping cart requested is superior to the existing ammount of the product.";
                    return RedirectToAction("Product", "Product", new { id = p.Id });
                }
                product.Ammount += temp.Ammount;
                productInCartRepository.Delete(new Tuple<long, long>(product.ProductId, product.ShoppingCartId));
                productInCartRepository.AddEntity(product);
            }
            else
                productInCartRepository.AddEntity(product);

            return RedirectToAction("Product", "Product", new { id = p.Id });
        }

        [HttpPost]
        public IActionResult AddProductById(AllProductsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["AmmountError"] = "Invalid";
                TempData["ProductId"] = viewModel.ProductId;
                return RedirectToAction("All", "Product");
            }
            
            var user = userRepository.GetByUsername(User.Identity.Name);
            var product = productRepository.GetById(viewModel.ProductId);
            if(user.Id == product.UserId)
            {
                TempData["UserError"] = "Invalid";
                TempData["ProductId"] = viewModel.ProductId;
                return RedirectToAction("All", "Product");
            }

            var cart = repository.GetById(user.ShoppingCartId);
            ProductInCart productInCart = new ProductInCart()
            {
                Ammount = viewModel.Ammount,
                ProductId = viewModel.ProductId,
                ShoppingCartId = cart.Id
            };

            var p = productInCartRepository.GetById(new Tuple<long, long>(viewModel.ProductId, cart.Id));
            if (p != null)
            {
                p.Ammount+=viewModel.Ammount;
                productInCartRepository.Update(p);
            }
            else
            {
                productInCartRepository.AddEntity(productInCart);
            }
            return RedirectToAction("All", "Product");
        }

        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            UserId = userRepository.GetByUsername(User.Identity.Name);
            productInCartRepository.Delete( new Tuple<long,long>((long)id, UserId.ShoppingCartId));
            return RedirectToAction("ShoppingCart");
        }

        private IEnumerable<Product> Products()
        {
            UserId = userRepository.GetByUsername(User.Identity.Name);
            IEnumerable<ProductInCart> products = productInCartRepository.GetByCartId(UserId.ShoppingCartId);
            List<Product> list = new List<Product>();
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
                    Id = product.Id,
                    Description = product.Description,
                    UserId = UserId.Id
                };
                list.Add(p);
            }
            return list;
        }

        public IActionResult ShoppingCart()
        {
            var list = Products();
            return View(list);
        }
                
        public IActionResult Checkout()
        {
            if (TempData["Error"] != null)
                ViewBag.Error = TempData["Error"];
            UserId = userRepository.GetByUsername(User.Identity.Name);
            var banks = bankAccountRepository.GetByUserId(UserId.Id);
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in banks)
            {
                list.Add(new SelectListItem { Value = item.Id.ToString(), Text = (item.AccountId, item.Titular).ToString() });
            }
            CheckoutViewModel viewModel = new CheckoutViewModel()
            {       
                BankAccounts = banks,
                Products = Products().ToList(),
                ListItems = list
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Checkout(CheckoutViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Checkout");

            UserId = userRepository.GetByUsername(User.Identity.Name);
            List<BankAccount> UsersToPay = new List<BankAccount>();
            List<float> MoneyToPay = new List<float>();
            //Se supone que se haga la transferencia bancaria y se compruebe que es valida
            
            foreach (var item in viewModel.Products)
            {
                //Encontrar el producto que se esta comprando
                Product product = productRepository.GetById(item.Id);
                if(product == null)
                {
                    TempData["Error"] = $"The product {item.Name} is no longer available";
                    return RedirectToAction("Checkout");
                }
                if(item.Ammount > product.Ammount)
                {
                    TempData["Error"] = $"Sorry! The product {item.Name} is out of order";
                    return RedirectToAction("Checkout");
                }

                //Actualizar el producto y guardar las notificaciones
                product.Ammount -= item.Ammount;
                if (product.Ammount == 0)
                {
                    product.Active = false;
                    Notification notification2 = new Notification(NotificationType.SaleCompleted, product.UserId, new object[] 
                    { UserId.Name, item.Name, item.Price,item.Ammount,item.Price * item.Ammount });                    
                    notificationRepository.AddEntity(notification2);
                }


                productRepository.Update(product);
                Notification notification = new Notification(NotificationType.SaleProduct, product.UserId, UserId.Name,
                    item.Name, item.Price, item.Ammount, item.Price * item.Ammount);

                Notification notification1 = new Notification(NotificationType.BuyProduct, UserId.Id, UserId.Name, item.Name, item.Price, item.Ammount, item.Price * item.Ammount);
               
                notificationRepository.AddEntity(notification);
                notificationRepository.AddEntity(notification1);

                //Insertar la venta
                Sale sale = new Sale()
                {
                    Ammount = item.Ammount,
                    Import = item.Price * item.Ammount,
                    Date = DateTime.Now,
                    ProductId = item.Id,
                    User_Buy_ID = UserId.Id,
                    User_Sale_ID = product.UserId,
                    Active = true
                };
                saleRepository.AddEntity(sale);
                if(bankAccountRepository.GetAll().Any(x => x.UserId == product.UserId))
                {
                    BankAccount account = bankAccountRepository.GetByUserId(product.UserId).First();
                    UsersToPay.Add(account);
                    MoneyToPay.Add((float)item.Price * item.Ammount);   
                }
                
            }

            List<ProductInCart> productsInCart = productInCartRepository.GetByCartId(UserId.ShoppingCartId).ToList();
            List<ProductInCart> productsToDelete = new List<ProductInCart>();
            foreach (var item in productsInCart)
            {
                productsToDelete.Add(item);
            }
            foreach (var item in productsToDelete)
            {               
                productInCartRepository.Delete(new Tuple<long,long>(item.ProductId,item.ShoppingCartId));
            }
            UsersToPay.Add(bankAccountRepository.GetByTitular("Shoppify"));
            MoneyToPay.Add((MoneyToPay.Sum() * 2) / 100);
            BankAccount userpay = bankAccountRepository.GetByUserId(UserId.Id).First();
            
            bool deposit = Bank.Deposit(userpay, UsersToPay, MoneyToPay);
            if (!deposit)
            {
                TempData["Error"] = "Error in the bank transfer.";               
            }
            TempData["Error"] = "Succesfull checkout";
            return RedirectToAction("Checkout");
        }
    }
}