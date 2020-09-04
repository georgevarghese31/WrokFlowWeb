using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using WrokFlowWeb.Areas.Identity.Data;
using WrokFlowWeb.ViewModel;
using WrokFlowWeb.ViewModel.Role;

namespace WrokFlowWeb.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<WrokFlowWebUser> userManager;

        public RoleController(RoleManager<IdentityRole> roleManager,
                              UserManager<WrokFlowWebUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        // GET: RoleController
        public ActionResult ListRoles()
        {
            return View(roleManager.Roles);
        }

        // GET: RoleController/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }

            var model = new EditRoleViewModel()
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

            return View("Edit",model);
        }

        // GET: RoleController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole()
                {
                    Name = roleViewModel.RoleName
                };
                IdentityResult identityResult = await roleManager.CreateAsync(identityRole);
                if (identityResult.Succeeded)
                {
                    return RedirectToAction(nameof(ListRoles));
                }

                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
            }
            return View(roleViewModel);

        }

        // GET: RoleController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RoleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditRoleViewModel editRoleViewModel)
        {
            var role = await roleManager.FindByIdAsync(editRoleViewModel.Id);
            if (role == null) {
                ViewBag.ErrorMessage = $"Role with Id = {editRoleViewModel.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = editRoleViewModel.RoleName;
                var result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(ListRoles));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty,error.Description);
                }
            }

          

            return View(editRoleViewModel);
        }

        // GET: RoleController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RoleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleid)
        {
            var role = await roleManager.FindByIdAsync(roleid);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleid} cannot be found";
                return View("NotFound");
            }

            var model = new UserRoleViewModel() { 
                RoleId = roleid
            };
            

            foreach (var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleList()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                model.UserRoleList.Add(userRoleViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(UserRoleViewModel userRoleViewModel)
        {
            var role = await roleManager.FindByIdAsync(userRoleViewModel.RoleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {userRoleViewModel.RoleId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < userRoleViewModel.UserRoleList.Count; i++)
            {
                var user = await userManager.FindByIdAsync(userRoleViewModel.UserRoleList[i].UserId);
                IdentityResult identityResult = null;
                if (userRoleViewModel.UserRoleList[i].IsSelected &&  !(await userManager.IsInRoleAsync(user,role.Name)))
                {
                    identityResult = await userManager.AddToRoleAsync(user,role.Name);
                }else if (!userRoleViewModel.UserRoleList[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    identityResult = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
                if (identityResult.Succeeded) {
                    if (i< (userRoleViewModel.UserRoleList.Count-1))
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("Details", new { id = userRoleViewModel.RoleId });
                    }
                }
            }
            return RedirectToAction("Details", new { id = userRoleViewModel.RoleId });
        }
    }
}
