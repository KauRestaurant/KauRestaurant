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
    // Restricts access to users with specific admin roles
    [Authorize(Roles = "A1")]
    public class UserManagementController : Controller
    {
        // Manages user operations (create, delete, roles, etc.)
        private readonly UserManager<KauRestaurantUser> _userManager;
        // Manages roles (e.g., creation, removal)
        private readonly RoleManager<IdentityRole> _roleManager;

        // Constructor injecting user and role managers
        public UserManagementController(
            UserManager<KauRestaurantUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Displays a list of existing admin users and provides a view model for creating new admins
        public async Task<IActionResult> Index()
        {
            // Fetch every user from the database
            var users = await _userManager.Users.ToListAsync();
            // Store user data for easier display
            var userViewModels = new List<UserViewModel>();
            // Predefined admin roles
            var adminRoles = new[] { "A1", "A2", "A3" };

            // Check each user's assigned roles
            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var adminRole = userRoles.FirstOrDefault(r => adminRoles.Contains(r));
                // Track only users in any valid admin role
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

            // Combine the admin list and a new-user form into a single model
            var model = Tuple.Create(userViewModels, new CreateUserViewModel());

            // Render the admin management page
            return View("~/Views/Admin/UserManagement.cshtml", model);
        }

        // Handles bulk role changes for multiple users
        [HttpPost]
        public async Task<IActionResult> SaveRoles(List<string> userIds, List<string> roles)
        {
            // Validate we have matching user and role lists
            if (userIds == null || roles == null || userIds.Count != roles.Count)
            {
                TempData["ErrorMessage"] = "حدث خطأ: البيانات المرسلة غير صحيحة";
                return RedirectToAction(nameof(Index));
            }

            bool success = true;
            var adminRoles = new[] { "A1", "A2", "A3" };
            var successCount = 0;

            // Loop over users and update their assigned roles
            for (int i = 0; i < userIds.Count; i++)
            {
                var userId = userIds[i];
                var newRole = roles[i];

                // Ignore non-admin roles
                if (!adminRoles.Contains(newRole))
                    continue;

                // Fetch user by ID
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    continue;

                // Skip modifying the currently logged-in user
                if (User.Identity.Name == user.Email)
                    continue;

                // Remove existing roles and add new role
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

            // Provide feedback to user regarding success/failure
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

        // Creates a new admin user with given properties
        [HttpPost]
        public async Task<IActionResult> CreateUser([Bind(Prefix = "Item2")] CreateUserViewModel model)
        {
            var adminRoles = new[] { "A1", "A2", "A3" };
            // Verify the selected role is a valid admin role
            if (!adminRoles.Contains(model.Role))
            {
                TempData["ErrorMessage"] = "مستوى الصلاحية غير صالح";
                return RedirectToAction(nameof(Index));
            }

            // Ensure the email is not already taken
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                TempData["ErrorMessage"] = "البريد الإلكتروني مستخدم بالفعل";
                return RedirectToAction(nameof(Index));
            }

            // Create the user if inputs are valid
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
                    // Assign requested admin role
                    await _userManager.AddToRoleAsync(user, model.Role);
                    TempData["SuccessMessage"] = "تم إنشاء المشرف بنجاح";
                    return RedirectToAction(nameof(Index));
                }

                // Collect any errors from the identity system
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            // Creation process failed
            TempData["ErrorMessage"] = "فشل إنشاء المشرف";
            return RedirectToAction(nameof(Index));
        }

        // Deletes the specified admin user
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            // If user not found, exit
            if (user == null)
            {
                TempData["ErrorMessage"] = "المشرف غير موجود";
                return RedirectToAction(nameof(Index));
            }

            // Confirm the user indeed has an admin role
            var adminRoles = new[] { "A1", "A2", "A3" };
            var userRoles = await _userManager.GetRolesAsync(user);

            if (!userRoles.Any(r => adminRoles.Contains(r)))
            {
                TempData["ErrorMessage"] = "هذا المستخدم ليس مشرفاً";
                return RedirectToAction(nameof(Index));
            }

            // Prevent deleting own account
            if (User.Identity.Name == user.Email)
            {
                TempData["ErrorMessage"] = "لا يمكنك حذف حسابك الحالي";
                return RedirectToAction(nameof(Index));
            }

            // Attempt to remove the user
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

    // Holds user data (ID, Name, Role) for display
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CurrentRole { get; set; }
    }

    // Encapsulates user creation fields and associated validation
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
