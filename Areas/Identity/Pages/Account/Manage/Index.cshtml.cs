// Areas/Identity/Pages/Account/Manage/Index.cshtml.cs
#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using KauRestaurant.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KauRestaurant.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<KauRestaurantUser> _userManager;
        private readonly SignInManager<KauRestaurantUser> _signInManager;

        public IndexModel(
            UserManager<KauRestaurantUser> userManager,
            SignInManager<KauRestaurantUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "الاسم الأول مطلوب")]
            [StringLength(50, MinimumLength = 2, ErrorMessage = "الاسم الأول يجب أن يكون بين {2} و {1} حرفاً")]
            [RegularExpression(@"^[\u0600-\u06FF\s]{2,50}$", ErrorMessage = "الاسم الأول يجب أن يحتوي على حروف عربية فقط")]
            [Display(Name = "الاسم الأول")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "اسم العائلة مطلوب")]
            [StringLength(50, MinimumLength = 2, ErrorMessage = "اسم العائلة يجب أن يكون بين {2} و {1} حرفاً")]
            [RegularExpression(@"^[\u0600-\u06FF\s]{2,50}$", ErrorMessage = "اسم العائلة يجب أن يحتوي على حروف عربية فقط")]
            [Display(Name = "اسم العائلة")]
            public string LastName { get; set; }

            [Phone]
            [Display(Name = "رقم الجوال")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(KauRestaurantUser user)
        {
            Username = await _userManager.GetUserNameAsync(user);
            Input = new InputModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = await _userManager.GetPhoneNumberAsync(user)
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("تعذر تحميل المستخدم.");

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("تعذر تحميل المستخدم.");

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            // Only admins can update phone
            if (User.IsInRole("A1") || User.IsInRole("A2") || User.IsInRole("A3"))
            {
                var currentPhone = await _userManager.GetPhoneNumberAsync(user);
                if (Input.PhoneNumber != currentPhone)
                {
                    var setPhone = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                    if (!setPhone.Succeeded)
                    {
                        StatusMessage = "Unexpected error when trying to set phone number.";
                        return RedirectToPage();
                    }
                }
            }

            // Always update names
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                StatusMessage = "حدث خطأ غير متوقع عند محاولة تحديث الملف الشخصي.";
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "تم تحديث معلوماتك الشخصية بنجاح";
            return RedirectToPage();
        }
    }
}
