using IdentityProject.Models;
using IdentityProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityProject.Controllers
{
    public class RoleController : Controller
    {
        RoleManager<CustomRole> _roleManager;

        public RoleController(RoleManager<CustomRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public IActionResult Index() => View(_roleManager.Roles.ToList());

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel role)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await _roleManager.CreateAsync(new CustomRole(role.Name, role.Description));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(role.Name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            CustomRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }
    }
}
