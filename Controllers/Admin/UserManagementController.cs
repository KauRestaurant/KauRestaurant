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

            // Build tuple model (user list, new create user model)
            var model = Tuple.Create(userViewModels, new CreateUserViewModel());

            // Explicitly return the view located at ~/Views/Admin/UserManagement.cshtml 
            return View("~/Views/Admin/UserManagement.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveRoles(List<string> userIds, List<string> roles)
        {
            if (userIds == null || roles == null || userIds.Count != roles.Count)
            {
                TempData["ErrorMessage"] = "حدث خطأ: البيانات المرسلة غير صحيحة";
                return RedirectToAction(nameof(Index));
            }

            bool success = true;
            var adminRoles = new[] { "A1", "A2", "A3" };
            var successCount = 0;

            for (int i = 0; i < userIds.Count; i++)
            {
                var userId = userIds[i];
                var newRole = roles[i];

                // Skip if not a valid admin role
                if (!adminRoles.Contains(newRole))
                    continue;

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    continue;

                // Skip if trying to modify current user's role
                if (User.Identity.Name == user.Email)
                    continue;

                var currentRoles = await _userManager.GetRolesAsync(user);
                var currentAdminRole = currentRoles.FirstOrDefault(r => adminRoles.Contains(r));

                if (currentAdminRole != newRole)
                {
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    var roleResult = await _userManager.AddToRoleAsync(user, newRole);

                    if (roleResult.Succeeded)
                    {
                        successCount++;
                    }
                    else
                    {
                        success = false;
                    }
                }
            }

            if (success)
            {
                if (successCount > 0)
                    TempData["SuccessMessage"] = $"تم تحديث صلاحيات {successCount} مشرف بنجاح";
                else
                    TempData["SuccessMessage"] = "لم يتم إجراء أي تغييرات";
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحديث بعض الصلاحيات";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([Bind(Prefix = "Item2")] CreateUserViewModel model)
        {
            // Validate that the role is an admin role
            var adminRoles = new[] { "A1", "A2", "A3" };
            if (!adminRoles.Contains(model.Role))
            {
                TempData["ErrorMessage"] = "مستوى الصلاحية غير صالح";
                return RedirectToAction(nameof(Index));
            }

            // Check if email is already in use
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                TempData["ErrorMessage"] = "البريد الإلكتروني مستخدم بالفعل";
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
