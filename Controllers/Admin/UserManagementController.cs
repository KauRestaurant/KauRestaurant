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
        public async Task<IActionResult> UpdateName(string userId, string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                TempData["ErrorMessage"] = "الاسم الأول والأخير مطلوبان";
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "المستخدم غير موجود";
                return RedirectToAction(nameof(Index));
            }

            user.FirstName = firstName;
            user.LastName = lastName;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "تم تحديث اسم المستخدم بنجاح";
            }
            else
            {
                TempData["ErrorMessage"] = "فشل تحديث اسم المستخدم";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmail(string userId, string newEmail)
        {
            if (string.IsNullOrEmpty(newEmail))
            {
                TempData["ErrorMessage"] = "البريد الإلكتروني مطلوب";
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "المستخدم غير موجود";
                return RedirectToAction(nameof(Index));
            }

            // Check if email is already in use
            var existingUser = await _userManager.FindByEmailAsync(newEmail);
            if (existingUser != null && existingUser.Id != userId)
            {
                TempData["ErrorMessage"] = "البريد الإلكتروني مستخدم بالفعل";
                return RedirectToAction(nameof(Index));
            }

            user.Email = newEmail;
            user.UserName = newEmail; // Since email is used as username
            user.NormalizedEmail = newEmail.ToUpper();
            user.NormalizedUserName = newEmail.ToUpper();

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "تم تحديث البريد الإلكتروني بنجاح";

                // If changing the current user's email, sign them out
                if (User.Identity.Name == user.Email)
                {
                    return RedirectToAction("Logout", "Account", new { area = "Identity" });
                }
            }
            else
            {
                TempData["ErrorMessage"] = "فشل تحديث البريد الإلكتروني";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string userId, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrEmpty(newPassword) || newPassword.Length < 6)
            {
                TempData["ErrorMessage"] = "كلمة المرور يجب أن تكون 6 أحرف على الأقل";
                return RedirectToAction(nameof(Index));
            }

            if (newPassword != confirmPassword)
            {
                TempData["ErrorMessage"] = "كلمات المرور غير متطابقة";
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "المستخدم غير موجود";
                return RedirectToAction(nameof(Index));
            }

            // Remove current password and set the new one
            var removePasswordResult = await _userManager.RemovePasswordAsync(user);
            if (!removePasswordResult.Succeeded)
            {
                TempData["ErrorMessage"] = "فشل تغيير كلمة المرور";
                return RedirectToAction(nameof(Index));
            }

            var addPasswordResult = await _userManager.AddPasswordAsync(user, newPassword);
            if (addPasswordResult.Succeeded)
            {
                TempData["SuccessMessage"] = "تم تغيير كلمة المرور بنجاح";
            }
            else
            {
                TempData["ErrorMessage"] = "فشل تغيير كلمة المرور";
                foreach (var error in addPasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

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
