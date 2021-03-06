using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.Name
                };

                var result = await _roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = _userManager.Users;
            return View(users);
        }
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {

            var role = await _roleManager.FindByIdAsync(id);
            if(role == null)
            {
                ViewBag.ErrorMessage = $"Role with {id} not found";
                return View("NotFound");
            }
            else
            {
                EditRoleViewModel model = new EditRoleViewModel
                {
                    Id = role.Id,
                    Name = role.Name
                };

                var appUsers = await _userManager.GetUsersInRoleAsync(role.Name);
                model.UserNames = new List<string>();
                foreach(var user in appUsers)
                {
                    model.UserNames.Add(user.UserName);
                }

                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);
                if (role == null)
                {
                    ViewBag.ErrorMessage = $"Role with {model.Id} not found";
                    return View("NotFound");
                }
                else
                {
                    role.Name = model.Name;
                    var result = await _roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string id)
        {
            ViewBag.RoleId = id;
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with {id} not found";
                return View("NotFound");
            }
            var modelList = new List<EditUsersInRoleViewModel>();
            foreach (var user in _userManager.Users)
            {
                EditUsersInRoleViewModel model = new EditUsersInRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.IsInRole = true;
                }
                else
                {
                    model.IsInRole = false;
                }
                modelList.Add(model);
            }
            return View(modelList);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<EditUsersInRoleViewModel> model,string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with {id} not found";
                return View("NotFound");
            }

            for(int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);
                if (model[i].IsInRole && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!(model[i].IsInRole) && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
            }
            return RedirectToAction("EditRole", new { id = role.Id });
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with {id} not found";
                return View("NotFound");
            }
            else
            {
                var claims = await _userManager.GetClaimsAsync(user);
                var roles = await _userManager.GetRolesAsync(user);
                EditUserViewModel model = new EditUserViewModel
                {
                    Id = user.Id,
                    Name=user.UserName,
                    Email = user.Email,
                    City = user.City,
                    Claims = claims.Select(c => c.Value).ToList(),
                    Roles = new List<string>(roles)
                };

                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with {model.Id} not found";
                return View("NotFound");
            }
            else
            {
                user.UserName = model.Name;
                user.Email = model.Email;
                user.City = model.City;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with {id} not found";
                return View("NotFound");
            }
            else
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return RedirectToAction("ListUsers");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with {id} not found";
                return View("NotFound");
            }
            else
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return RedirectToAction("ListRoles");
            }
        }
    }
}
