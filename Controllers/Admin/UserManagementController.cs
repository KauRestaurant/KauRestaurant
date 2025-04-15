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
        public async Task<IActionResult> UpdateUser(string userId, string firstName, string lastName, string newEmail, string newRole)
        {
            // Check if user exists
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "المستخدم غير موجود";
                return RedirectToAction(nameof(Index));
            }

            bool needsSignOut = false;
            bool success = true;
            List<string> successMessages = new List<string>();
            List<string> errorMessages = new List<string>();

            // Update name if changed
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName) &&
                (user.FirstName != firstName || user.LastName != lastName))
            {
                user.FirstName = firstName;
                user.LastName = lastName;

                var nameResult = await _userManager.UpdateAsync(user);
                if (nameResult.Succeeded)
                {
                    successMessages.Add("تم تحديث الاسم بنجاح");
                }
                else
                {
                    success = false;
                    errorMessages.Add("فشل تحديث الاسم");
                }
            }

            // Update email if changed
            if (!string.IsNullOrEmpty(newEmail) && user.Email != newEmail)
            {
                // Check if email is already in use
                var existingUser = await _userManager.FindByEmailAsync(newEmail);
                if (existingUser != null && existingUser.Id != userId)
                {
                    success = false;
                    errorMessages.Add("البريد الإلكتروني مستخدم بالفعل");
                }
                else
                {
                    user.Email = newEmail;
                    user.UserName = newEmail; // Since email is used as username
                    user.NormalizedEmail = newEmail.ToUpper();
                    user.NormalizedUserName = newEmail.ToUpper();

                    var emailResult = await _userManager.UpdateAsync(user);
                    if (emailResult.Succeeded)
                    {
                        successMessages.Add("تم تحديث البريد الإلكتروني بنجاح");

                        // If changing the current user's email, sign them out after all updates
                        if (User.Identity.Name == user.Email)
                        {
                            needsSignOut = true;
                        }
                    }
                    else
                    {
                        success = false;
                        errorMessages.Add("فشل تحديث البريد الإلكتروني");
                    }
                }
            }

            // Update role if changed
            var adminRoles = new[] { "A1", "A2", "A3" };
            if (!string.IsNullOrEmpty(newRole) && adminRoles.Contains(newRole))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                var currentAdminRole = currentRoles.FirstOrDefault(r => adminRoles.Contains(r));

                if (currentAdminRole != newRole)
                {
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    var roleResult = await _userManager.AddToRoleAsync(user, newRole);

                    if (roleResult.Succeeded)
                    {
                        successMessages.Add("تم تحديث الصلاحية بنجاح");
                    }
                    else
                    {
                        success = false;
                        errorMessages.Add("فشل تحديث الصلاحية");
                    }
                }
            }

            // Set appropriate messages
            if (success)
            {
                if (successMessages.Count > 0)
                {
                    TempData["SuccessMessage"] = string.Join("، ", successMessages);
                }
                else
                {
                    TempData["SuccessMessage"] = "لم يتم إجراء أي تغييرات";
                }
            }
            else
            {
                if (errorMessages.Count > 0)
                {
                    TempData["ErrorMessage"] = string.Join("، ", errorMessages);
                }
            }

            // Redirect to logout if current user's email was changed
            if (needsSignOut)
            {
                return RedirectToAction("Logout", "Account", new { area = "Identity" });
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
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "الرجاء إدخال بريد إلكتروني صحيح")]
        public string Email { get; set; }

        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        [StringLength(50, ErrorMessage = "الاسم الأول يجب أن يكون أقل من {1} حرف")]
        [RegularExpression(@"^[a-zA-Zأ-يءئؤلإآ\s]*$", ErrorMessage = "الاسم الأول يجب أن يحتوي على أحرف فقط")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "الاسم الأخير مطلوب")]
        [StringLength(50, ErrorMessage = "الاسم الأخير يجب أن يكون أقل من {1} حرف")]
        [RegularExpression(@"^[a-zA-Zأ-يءئؤلإآ\s]*$", ErrorMessage = "الاسم الأخير يجب أن يحتوي على أحرف فقط")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "كلمة المرور يجب أن تكون {2} أحرف على الأقل")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "مستوى الصلاحية مطلوب")]
        public string Role { get; set; }
    }

}
