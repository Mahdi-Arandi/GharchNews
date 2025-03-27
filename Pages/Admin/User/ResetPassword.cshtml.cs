
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using GharchNews.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using GharchNews.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using static System.Runtime.InteropServices.JavaScript.JSType;
using GharchNews.Roles;

namespace GharchNews.Pages.Admin.User
{
    [Authorize(Roles = SD.User)]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ResetPasswordModel(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {


            [DataType(DataType.Password)]
            [Display(Name = "کلمه عبور فعلی")]
            public string CurrentPassword { get; set; }

            [Required(ErrorMessage = "لطفا کلمه عبور را وارد کنید")]
            [StringLength(100, ErrorMessage = "{0} باید حداقل {2} کاراکتر و حداکثر {1} کاراکتر داشته باشد.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "کلمه عبور جدید")]
            public string NewPassword { get; set; }



            [DataType(DataType.Password)]
            [Display(Name = "تکرار کلمه عبور")]
            [Compare("NewPassword", ErrorMessage = "کلمه عبور با تکرار کلمه عبور تطابق ندارد.")]
            public string ConfirmPassword { get; set; }

        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var myUser = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (myUser == null)
            {
                return NotFound();
            }

            var currentPasswordHashed = _userManager.PasswordHasher.VerifyHashedPassword(myUser, myUser.PasswordHash, Input.CurrentPassword);

            if (currentPasswordHashed == PasswordVerificationResult.Success)
            {
                myUser.PasswordHash = _userManager.PasswordHasher.HashPassword(myUser, Input.NewPassword);

                _context.Update(myUser);
                await _context.SaveChangesAsync();
                await HttpContext.SignOutAsync();
                return RedirectToPage("/Login");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "کلمه عبور نادرست است.");
                return Page();
            }
        }
    }
}
