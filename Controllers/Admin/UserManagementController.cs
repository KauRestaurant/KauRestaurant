using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KauRestaurant.Areas.Identity.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KauRestaurant.Controllers.Admin
{
    [Authorize(Roles = "A1")]
    public class UserManagementController : Controller
    {
        private readonly UserManager<KauRestaurantUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManagementController(
            UserManager<KauRestaurantUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            // Get all users
            var users = await _userManager.Users.ToListAsync();
            var userViewModels = new List<UserViewModel>();
            var adminRoles = new[] { "A1", "A2", "A3" };

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                // Filter only users with admin roles (A1, A2, A3)
                var adminRole = userRoles.FirstOrDefault(r => adminRoles.Contains(r));

                if (adminRole != null)
                {
                    userViewModels.Add(new UserViewModel
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        CurrentRole = adminRole
                    });
                }
            }

            return View("~/Views/Admin/UserManagement.cshtml", userViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(string userId, string newRole)
        {
            // Validate that the new role is an admin role
            var adminRoles = new[] { "A1", "A2", "A3" };
            if (!adminRoles.Contains(newRole))
            {
                TempData["ErrorMessage"] = "مستوى الصلاحية غير صالح";
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "المستخدم غير موجود";
                return RedirectToAction(nameof(Index));
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            // Verify this user has an admin role
            if (!currentRoles.Any(r => adminRoles.Contains(r)))
            {
                TempData["ErrorMessage"] = "هذا المستخدم غير مصرح له بالصلاحيات الإدارية";
                return RedirectToAction(nameof(Index));
            }

            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, newRole);

            TempData["SuccessMessage"] = "تم تغيير صلاحية المشرف بنجاح";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            // Validate that the role is an admin role
            var adminRoles = new[] { "A1", "A2", "A3" };
            if (!adminRoles.Contains(model.Role))
            {
                TempData["ErrorMessage"] = "مستوى الصلاحية غير صالح";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                var user = new KauRestaurantUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                    TempData["SuccessMessage"] = "تم إنشاء المشرف بنجاح";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            TempData["ErrorMessage"] = "فشل إنشاء المشرف";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "المشرف غير موجود";
                return RedirectToAction(nameof(Index));
            }

            // Make sure the user has an admin role
            var adminRoles = new[] { "A1", "A2", "A3" };
            var userRoles = await _userManager.GetRolesAsync(user);

            if (!userRoles.Any(r => adminRoles.Contains(r)))
            {
                TempData["ErrorMessage"] = "هذا المستخدم ليس مشرفاً";
                return RedirectToAction(nameof(Index));
            }

            // Check if trying to delete self
            if (User.Identity.Name == user.Email)
            {
                TempData["ErrorMessage"] = "لا يمكنك حذف حسابك الحالي";
                return RedirectToAction(nameof(Index));
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "تم حذف المشرف بنجاح";
            }
            else
            {
                TempData["ErrorMessage"] = "فشل حذف المشرف";
            }

            return RedirectToAction(nameof(Index));
        }
    }

    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CurrentRole { get; set; }
    }

    public class CreateUserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
