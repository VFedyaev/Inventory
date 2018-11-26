using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.BLL.Interfaces;
using Inventory.Web.Models.Account;
using Inventory.Web.Models.User;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;

namespace Inventory.Web.Controllers
{
    public class UserController : Controller
    {
        private const int ItemsPerPage = 10;

        private readonly IUserService UserService;
        private readonly IAccountService AccountService;
        
        public UserController(IUserService userService, IAccountService accountService)
        {
            UserService = userService;
            AccountService = accountService;
        }

        [Authorize(Roles = "admin")]
        public ActionResult Index(int? page)
        {
            var userDTOs = UserService.GetAllUsers().ToList();
            var userVMs = Mapper.Map<IEnumerable<UserVM>>(userDTOs).ToPagedList(page ?? 1, ItemsPerPage);

            return View(userVMs);
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> ChangeRole(string userId)
        {
            if (userId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var role = await UserService.GetUserRole(userId);
            ChangeRoleModel model = new ChangeRoleModel
            {
                UserId = userId,
                OldRole = role,
                Role = role
            };

            ViewBag.Role = GetRoleNameSelectList(model.Role);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeRole([Bind(Include = "UserId,OldRole,Role")] ChangeRoleModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ChangeRoleDTO changeRoleDTO = new ChangeRoleDTO
                    {
                        UserId = model.UserId,
                        OldRole = model.OldRole,
                        Role = model.Role
                    };
                    await UserService.ChangeUserRole(changeRoleDTO);
                    TempData["success"] = "Изменения сохранены.";
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Произошла ошибка. Попробуйте еще раз либо обратитесь к администратору.");
                }
            }
            ViewBag.Role = GetRoleNameSelectList(model.Role);

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await UserService.DeleteUser(id);
            }
            catch (ArgumentNullException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (Exception)
            {
                TempData["fail"] = "Произошла ошибка.";
            }

            return RedirectToAction("Index");
        }

        private SelectList GetRoleNameSelectList(string selectedValue = null)
        {
            return new SelectList(AccountService.GetAllRoles().ToList(), "Name", "Description", selectedValue);
        }
    }
}