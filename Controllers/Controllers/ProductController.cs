using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Project.Models;
using Project.ViewModels;

namespace Project.Controllers
{
    public class ProductController : Controller
    {
        public readonly IProductRepository repository;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IUserRepository userRepository;

        public ProductController(IProductRepository repository, IHostingEnvironment hostingEnvironment, IUserRepository userRepository)
        {
            this.repository = repository;
            this.hostingEnvironment = hostingEnvironment;
            this.userRepository = userRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult All(string filter = "", int page = 1)
        {
            if (TempData != null)
            {
                if (TempData.Keys.Contains("AmmountError") && TempData["AmmountError"] != null)
                {
                    ViewBag.ProductId = TempData["ProductId"];
                    ViewBag.AmmountError = TempData["AmmountError"];
                }
                if (TempData.Keys.Contains("UserError") && TempData["UserError"] != null)
                {
                    ViewBag.ProductId = TempData["ProductId"];
                    ViewBag.AmmountError = "You own this.";
                }
            }
            if (page <= 0) page = 1;
            AllProductsViewModel model = new AllProductsViewModel();
            if (page % 3 == 0)
                model.FirstPage = page - 2 ;
            else 
                model.FirstPage = page / 3 > 0 ? page / 3 * 3 + 1: 1;
            if (model.FirstPage <= 0) model.FirstPage = 1;
            model.Products = repository.GetAll().Skip((page - 1) * 9).Where(x => filter == "" || x.Category.ToString() == filter || filter == "All").Take(9).ToList();
            model.Page = page;
            model.selectListItems = new List<SelectListItem>();
            string[] array = { "None", "Price Down", "Price Up", "Rating Down", "Rating Up", "Name Down", "Name Up" };
            int count = 0;
            foreach (var item in Enum.GetNames(typeof(Filters)))
            {
                model.selectListItems.Add(new SelectListItem(array[count],item));
                ++count;
            }
            model.selectListCategory = new List<SelectListItem>();
            foreach (var item in Enum.GetNames(typeof(Category)))
            {
                model.selectListCategory.Add(new SelectListItem(item, item));
            }
            return View(model);
        }

        [HttpPost]
        public ViewResult AllFilter(AllProductsViewModel viewModel)
        {
            
            var s = viewModel.Filter;
            var categ = viewModel.Category;
            int count = 0;
            string[] array = { "None", "Price Down", "Price Up", "Rating Down", "Rating Up", "Name Down", "Name Up" };
            int page = viewModel.Page;

            foreach (var item in Enum.GetNames(typeof(Filters)))
            {
                if(s.ToString() == item)
                {
                    viewModel.selectListItems.Add(new SelectListItem(array[count], item,true));
                }
                else
                {
                    viewModel.selectListItems.Add(new SelectListItem(array[count], item));
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
                    viewModel.Products = repository.GetAll().Skip((page - 1) * 9).Where(x => categ == Category.All || categ == x.Category).Take(9).ToList();
                    break;
                case Filters.PriceDown:
                    viewModel.Products = repository.GetAll().Skip((page - 1) * 9).Where(x => categ == Category.All || categ == x.Category).OrderByDescending(x =>x.Price).Take(9).ToList();
                    break;
                case Filters.PriceUp:
                    viewModel.Products = repository.GetAll().Skip((page - 1) * 9).Where(x => categ == Category.All || categ == x.Category).OrderBy(x => x.Price).Take(9).ToList();
                    break;
                case Filters.RatingDown:
                    viewModel.Products = repository.GetAll().Skip((page - 1) * 9).Where(x => categ == Category.All || categ == x.Category).OrderByDescending(x => x.Rating).Take(9).ToList();
                    break;
                case Filters.RatingUp:
                    viewModel.Products = repository.GetAll().Skip((page - 1) * 9).Where(x => categ == Category.All || categ == x.Category).OrderBy(x => x.Rating).Take(9).ToList();
                    break;
                case Filters.NameDown:
                    viewModel.Products = repository.GetAll().Skip((page - 1) * 9).Where(x => categ == Category.All || categ == x.Category).OrderByDescending(x => x.Name).Take(9).ToList();
                    break;
                case Filters.NameUp:
                    viewModel.Products = repository.GetAll().Skip((page - 1) * 9).Where(x => categ == Category.All || categ == x.Category).OrderBy(x => x.Name).Take(9).ToList();
                    break;
                default:
                    break;
            }

            return View("All",viewModel);
        }

        public IActionResult Add()
        {
            var UserId = userRepository.GetByUsername(User.Identity.Name).Id;

            if (!userRepository.HasAccounts(UserId))
            {                
                return RedirectToAction("Add", "BankAccount");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Add(ProductCreateViewModel product)
        {
            var UserId = userRepository.GetByUsername(product.Username).Id;

            string uniquefilename = null;
            if (!ModelState.IsValid)
                return View();

          
            Product newproduct = new Product()
            {
                Name = product.Name,
                Ammount = product.Ammount ?? 1,
                Category = product.Category,
                Description = product.Description,
                Price = product.Price ?? 1,
                UserId = UserId
            };

            repository.AddEntity(newproduct);
            long id = newproduct.Id;

            string folder = Path.Combine(hostingEnvironment.WebRootPath, "images", id.ToString());
            if (!System.IO.File.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            if (product.Images != null && product.Images.Count > 0)
            {
                
                foreach (var image in product.Images)
                {
                    uniquefilename = Guid.NewGuid().ToString() + "_" + image.FileName;
                    string path = Path.Combine(folder, uniquefilename);
                    image.CopyTo(new FileStream(path, FileMode.Create));
                }
            }
            else
            {
                uniquefilename = "NoImage.png";
                string sourcePath = Path.Combine(hostingEnvironment.WebRootPath, "img",uniquefilename);
                folder = Path.Combine(folder, uniquefilename);
                System.IO.File.Copy(sourcePath, folder);
            }

            newproduct.Images = uniquefilename;
            repository.Update(newproduct);

            ProductViewModel productViewModel = new ProductViewModel();
            productViewModel.Product = repository.GetById(id);
            productViewModel.Reviews = repository.Reviews(id).ToList();
            //productViewModel.Favorites = repository.Favorites().ToList();

            //Hacer paginado para los reviews
            ViewData["ReviewPages"] = 1;
            //return RedirectToAction("product", newproduct.Id);

            return View("VisualizeProduct", productViewModel);
        }

        public IActionResult Delete(long Id,string url)
        {
            repository.Delete(Id);
            if(url == null)
                return RedirectToAction("UserProduct");
            return RedirectToAction("MyUser", "User");
        }

        [HttpGet]
        public ViewResult Update(long Id)
        {
            var item = repository.GetById(Id);
            ProductCreateViewModel p = new ProductCreateViewModel()
            {
                Id = Id,
                Ammount = item.Ammount,
                Category = item.Category,
                Description = item.Description,
                Name = item.Name,
                Price = item.Price,
                Rating = item.Rating,

            };
            
            string folder = Path.Combine(hostingEnvironment.WebRootPath, "images", item.Id.ToString());
            try
            {
                IEnumerable<string> files = Directory.EnumerateFiles(folder);
                p.img = new List<string>();
                foreach (var img in files)
                {
                    p.img.Add(img);
                }
            }
            catch { }

            return View(p);
        }
        [HttpPost]
        public IActionResult Update(ProductCreateViewModel item)
        {
            var user = userRepository.GetByUsername(User.Identity.Name);
            Product p = new Product()
            {
                Id = item.Id,
                Name = item.Name,
                Category = item.Category,
                Ammount = (int)item.Ammount,
                Description = item.Description,
                Price = (decimal)item.Price,
                Rating = item.Rating,
                Active = true,
                User = user,
                UserId = user.Id
            };

            string uniquefilename = null;
            string folder = Path.Combine(hostingEnvironment.WebRootPath, "images", item.Id.ToString());
            if (!System.IO.File.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            if (item.Images != null && item.Images.Count > 0)
            {
                foreach (var image in item.Images)
                {
                    uniquefilename = Guid.NewGuid().ToString() + "_" + image.FileName;
                    string path = Path.Combine(folder, uniquefilename);
                    image.CopyTo(new FileStream(path, FileMode.Create));
                }
            }
            else
            {
                uniquefilename = "NoImage.png";
                string sourcePath = Path.Combine(hostingEnvironment.WebRootPath, "img", uniquefilename);
                var file = Path.Combine(folder, uniquefilename);
                try
                {
                    System.IO.File.Copy(sourcePath, file);
                }
                catch {  }

               
            }

            IEnumerable<string> files = Directory.EnumerateFiles(folder);
            p.Images = uniquefilename;
            repository.Update(p);
            return RedirectToAction("UserProduct");
        }
        [AllowAnonymous]
        public ViewResult Product(long Id)
        {
            
            ViewBag.ReviewError = TempData["ReviewError"];
            ViewBag.AmmountError = TempData["AmmountError"];
            ProductViewModel model = new ProductViewModel();
            model.Product = repository.GetById(Id);
            model.Reviews = repository.Reviews(Id).ToList();
            model.Favorites = repository.Favorites().Take(9).ToList();
            model.Files = new DirectoryInfo($"wwwroot/images/{model.Product.Id}").GetFiles().Select(o => o.Name);
            model.ReviewPage = 1;
            model.ReviewFirstPage = 1;

            return View(model);
        }

        [AllowAnonymous]
        public ViewResult ProductReview(long Id, int page)
        {
            ProductViewModel viewModel = new ProductViewModel();
            if (page <= 0) page = 1;
            if (page % 3 == 0)
                viewModel.ReviewFirstPage = page - 2;
            else
                viewModel.ReviewFirstPage = page / 3 > 0 ? page / 3 * 3 + 1 : 1;
            if (viewModel.ReviewFirstPage <= 0) viewModel.ReviewFirstPage = 1;
            viewModel.ReviewPage = page;
            viewModel.Product = repository.GetById(Id);
            viewModel.Favorites = repository.Favorites().Take(9).ToList();
            viewModel.Files = new DirectoryInfo($"wwwroot/images/{viewModel.Product.Id}").GetFiles().Select(o => o.Name);
            viewModel.Reviews = repository.Reviews(viewModel.ProductId).Skip((viewModel.ReviewPage - 1) * 3).ToList();
            return View("Product",viewModel);
        }

        [HttpGet]
        public IActionResult UserProduct()
        {
            AllProductsViewModel model = new AllProductsViewModel();
            model.FirstPage = 1;
            string id = userRepository.GetByUsername(User.Identity.Name).Id;
            model.Products = repository.GetUserProducts(id).Take(9).ToList();
            model.selectListItems = new List<SelectListItem>();
            string[] array = { "None", "Price Down", "Price Up", "Rating Down", "Rating Up", "Name Down", "Name Up" };
            int count = 0;
            foreach (var item in Enum.GetNames(typeof(Filters)))
            {
                model.selectListItems.Add(new SelectListItem(array[count], item));
                ++count;
            }
            model.selectListCategory = new List<SelectListItem>();
            foreach (var item in Enum.GetNames(typeof(Category)))
            {
                model.selectListCategory.Add(new SelectListItem(item, item));
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult UserProduct(AllProductsViewModel viewModel)
        {
            int page = viewModel.Page;
            string category = viewModel.Category.ToString();
            if (page <= 0) page = 1;
            AllProductsViewModel model = new AllProductsViewModel();
            if (page % 3 == 0)
                model.FirstPage = page - 2;
            else
                model.FirstPage = page / 3 > 0 ? page / 3 * 3 + 1 : 1;
            if (model.FirstPage <= 0) model.FirstPage = 1;
            string id = userRepository.GetByUsername(User.Identity.Name).Id;
            model.Products = repository.GetUserProducts(id).Skip((page - 1) * 9).Where(x => category == "" || x.Category.ToString() == category || category == "All").ToList();
            model.selectListItems = new List<SelectListItem>();
            string[] array = { "None", "Price Down", "Price Up", "Rating Down", "Rating Up", "Name Down", "Name Up" };
            int count = 0;
            foreach (var item in Enum.GetNames(typeof(Filters)))
            {
                model.selectListItems.Add(new SelectListItem(array[count], item));
                ++count;
            }
            model.selectListCategory = new List<SelectListItem>();
            foreach (var item in Enum.GetNames(typeof(Category)))
            {
                model.selectListCategory.Add(new SelectListItem(item, item));
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult VisualizeProduct(long id)
        {
            var p = repository.GetById(id);
            ProductViewModel viewModel = new ProductViewModel()
            {
                Product = p,
                Files = new DirectoryInfo($"wwwroot/images/{p.Id}").GetFiles().Select(o => o.Name)
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult IlicitContent(ProductViewModel model)
        {
            var p = repository.GetById(model.Product.Id);
            if (p.IlicitContent)
                return RedirectToAction("Product", new { p.Id });
            p.IlicitContent = true;
            repository.Update(p);
            return RedirectToAction("Product", new { p.Id });
        }

        [HttpGet]
        public IActionResult IlicitProduct()
        {
            var products = repository.IlicitProducts();
            IlicitProductViewModel viewModel = new IlicitProductViewModel()
            {
                Products = products
            };
            return View(viewModel);
        }

        public IActionResult RemoveIlicit(string id)
        {
            Product p = repository.GetById(id);
            p.IlicitContent = false;
            repository.Update(p);
            return RedirectToAction("IlicitProduct");
        }

    }
}
