using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project.Controllers
{
    //[Authorize(Policy = "AdminRolePolicy")]
    [AllowAnonymous]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        private readonly IUserRepository repository;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IUserRepository repository)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.repository = repository;
        }

        [HttpGet]
        [Authorize(Policy = "ManageRolesAndClaimsPolicy")]
        public async Task<IActionResult> ManageUserRoles(string Id)
        {
            ViewBag.userId = Id;

            var user = await userManager.FindByIdAsync(Id);

            if(user == null)
            {
                ErrorViewModel mo = new ErrorViewModel();
                mo.ErrorTitle = $"User id {Id} not found";
                mo.ErrorMessage = $"Introduce a new user";
                return View("Error", mo);
            }

            var model = new List<UserRolesViewModel>();
            foreach(var role in roleManager.Roles)
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                }; 

                if (await userManager.IsInRoleAsync(user, role.Name))
                    userRolesViewModel.IsSelected = true;
                else
                    userRolesViewModel.IsSelected = false;
                model.Add(userRolesViewModel);
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "ManageRolesAndClaimsPolicy")]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string Id) 
        {
            var user = await userManager.FindByIdAsync(Id);

            if (user == null)
            {
                ErrorViewModel mo = new ErrorViewModel();
                mo.ErrorTitle = $"User id {Id} not found";
                mo.ErrorMessage = $"Introduce a new user";
                return View("Error", mo);
            }

            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);

            if(!result.Succeeded)
            {
                ErrorViewModel mo = new ErrorViewModel();
                mo.ErrorTitle = "";
                mo.ErrorMessage = "Cannot remove user existing roles";
                return View("Error", mo);
            }

            result = await userManager.AddToRolesAsync(user, 
                model.Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ErrorViewModel mo = new ErrorViewModel();
                mo.ErrorTitle = "";
                mo.ErrorMessage = "Cannot add roles to user";
                return View("Error", mo);
            }

            return RedirectToAction("EditUser", new { Id = Id });
        }

        [HttpGet]
        [Authorize(Policy = "ManageRolesAndClaimsPolicy")]
        public async Task<IActionResult> ManageUserClaims(string Id)
        {
            ViewBag.userId = Id;

            var user = await userManager.FindByIdAsync(Id);

            if (user == null)
            {
                ErrorViewModel mo = new ErrorViewModel();
                mo.ErrorTitle = $"User id {Id} not found";
                mo.ErrorMessage = $"Introduce a new user";
                return View("Error", mo);
            }

            var existingUserClaims = await userManager.GetClaimsAsync(user);
            var model = new List<UserClaimsViewModel>();

            foreach(Claim claim in ClaimsStore.AllClaims)
            {
                UserClaimsViewModel userClaim = new UserClaimsViewModel
                {
                    ClaimType = claim.Type
                };
                if (existingUserClaims.Any(x => x.Type == claim.Type && x.Value == "true"))               
                    userClaim.IsSelected = true; 

                model.Add(userClaim);
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "ManageRolesAndClaimsPolicy")]
        public async Task<IActionResult> ManageUserClaims(List<UserClaimsViewModel> model, string Id)
        {
            var user = await userManager.FindByIdAsync(Id);

            if (user == null)
            {
                ErrorViewModel mo = new ErrorViewModel();
                mo.ErrorTitle = $"User id {Id} not found";
                mo.ErrorMessage = $"Introduce a new user";
                return View("Error", mo);
            }

            var claims = await userManager.GetClaimsAsync(user);
            var result = await userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                ErrorViewModel mo = new ErrorViewModel();
                mo.ErrorTitle = "";
                mo.ErrorMessage = "Cannot remove user existing claims";
                return View("Error", mo);
            }

            result = await userManager.AddClaimsAsync(user, 
                model.Select(x => new Claim(x.ClaimType, x.IsSelected ? "true" : "false")));

            if (!result.Succeeded)
            {
                ErrorViewModel mo = new ErrorViewModel();
                mo.ErrorTitle = "";
                mo.ErrorMessage = "Cannot add selected claims to user";
                return View("Error", mo);
            }
            return RedirectToAction("EditUser", new { Id = Id });
        }

        [HttpGet]
        [Authorize(Policy = "CreateRolePolicy")]
        public IActionResult CreateRole()
        {
             return View();
        }

        [HttpPost] 
        [Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role is null)
                return RedirectToAction("AllRoles ");

            try
            {
                var result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("AllRoles");
                }
                foreach (var e in result.Errors)
                {
                    ModelState.AddModelError("", e.Description);
                }
                return RedirectToAction("AllRoles");
            }
            catch (DbUpdateException)
            {
                ErrorViewModel model = new ErrorViewModel();
                model.ErrorTitle = $"{role.Name} is in use";
                model.ErrorMessage = $"There are users using this role, you should delete them first";
                return View("Error", model);
            }
        }

        [HttpPost]
        [Authorize(Policy = "CreateRolePolicy")]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var r = await roleManager.FindByNameAsync(model.RoleName);

                if (r is null)
                {
                    ViewBag.RoleName = $"Role name {model.RoleName} is already in use";
                }

                IdentityRole role = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult result = await roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("AllRoles", "Administration");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }

        public IActionResult AllRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role is null)
                return RedirectToAction("Index", "Home");

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);
            if (role is null)
                return RedirectToAction("Index", "Home");
            role.Name = model.RoleName;
            var result = await roleManager.UpdateAsync(role);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                return View();
            }               
            return RedirectToAction("AllRoles", "Administration");
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string id)
        {
            ViewBag.RoleId = id;

            var role = await roleManager.FindByIdAsync(id);

            if (role is null)
                //Poner pagina o mesnsaje para mostrar los errores
                return RedirectToAction("AllRoles", "Administration");

            var model = new List<UserRoleViewModel>();

            foreach (var user in userManager.Users)
            {
                if (!user.Active) continue;
                var userrole = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                    userrole.IsSelected = true;
                else
                    userrole.IsSelected = false;
                model.Add(userrole);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role is null)
                //Poner pagina o mesnsaje para mostrar los errores
                return RedirectToAction("AllRoles", "Administration");

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                    continue;

                if (result.Succeeded && i == model.Count - 1)
                {
                    return RedirectToAction("EditRole", new { Id = id });
                }
            }
            return RedirectToAction("EditRole", new { Id = id });
        }

        [HttpGet]
        public IActionResult Users()
        {
            var users = userManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if(user is null)
                return RedirectToAction("Users");

            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);


            var model = new EditUserViewModel
            {

                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Claims = userClaims.Select(c => c.Type + ": " + c.Value).ToList(),
                Roles = userRoles.ToList(),
                City = user.City,
                Info = user.Info,
                UserName = user.UserName,
                Telephone = user.PhoneNumber
            };

            return View(model);

        }
    

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);
            if (user is null)
                return RedirectToAction("Users");

            user.Email = model.Email;
            user.UserName = model.UserName;
            user.Name = model.Name;
            user.LastName = model.LastName;
            user.PhoneNumber = model.Telephone;
            user.City = model.City;            

            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Users");
            }
            foreach(var e in result.Errors)
            {
                ModelState.AddModelError("", e.Description);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user is null)
                return RedirectToAction("Users");

            repository.Delete(id);

            //var result = await userManager.Users.First(x => x.Id == Id)
            //if (result.Succeeded)
            //{
            //    return RedirectToAction("Users");
            //}
            //foreach (var e in result.Errors)
            //{
            //    ModelState.AddModelError("", e.Description);
            //}
            return RedirectToAction("Users");
        }

    }
}
