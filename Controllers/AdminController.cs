using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using InsuranceWebApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceWebApp.Controllers
{
    public class UserRoleViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public List<string> Roles { get; set; }
    }

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Users()
        {
            var users = _userManager.Users.ToList();
            var model = new List<UserRoleViewModel>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                model.Add(new UserRoleViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    Roles = roles.ToList()
                });
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(role))
            {
                TempData["Error"] = "Не указан пользователь или роль";
                return RedirectToAction(nameof(Users));
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "Пользователь не найден";
                return RedirectToAction(nameof(Users));
            }

            if (!await _roleManager.RoleExistsAsync(role))
            {
                TempData["Error"] = "Роль не существует";
                return RedirectToAction(nameof(Users));
            }

            // Получаем текущие роли
            var currentRoles = await _userManager.GetRolesAsync(user);
            // Удаляем все текущие роли
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                TempData["Error"] = "Не удалось удалить старые роли";
                return RedirectToAction(nameof(Users));
            }

            // Добавляем новую роль
            var addResult = await _userManager.AddToRoleAsync(user, role);
            if (addResult.Succeeded)
            {
                TempData["Success"] = $"Роль '{role}' назначена пользователю {user.Email}";
            }
            else
            {
                TempData["Error"] = "Ошибка при назначении роли";
            }

            return RedirectToAction(nameof(Users));
        }
    }
}