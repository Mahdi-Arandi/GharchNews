using GharchNews.Data;
using GharchNews.Models;
using GharchNews.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DiaSymReader;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GharchNews.Pages.Admin.User
{
    [Authorize(Roles = SD.Admin)]
    public class EditModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;


        public EditModel(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }


        [BindProperty]
        public InputModel Input { get; set; }


        public IFormFile? imgUp { get; set; }

        public class InputModel
        {
            public string? SelectedRole { get; set; }

            [StringLength(100, ErrorMessage = "{0} ???? ????? {2} ??????? ? ?????? {1} ??????? ????? ????.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "???? ????")]
            public string? Password { get; set; }


            [DataType(DataType.Password)]
            [Display(Name = "????? ???? ????")]
            [Compare("Password", ErrorMessage = "???? ???? ?? ????? ???? ???? ????? ?????.")]
            public string? ConfirmPassword { get; set; }

        }

        [BindProperty]
        public Users MyUser { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id.Trim().Length == 0)
            {
                return NotFound();
            }

            MyUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (MyUser == null)
            {
                return NotFound();
            }

            initRoles();
            return Page();
        }


        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            user.Name = MyUser.Name;
            user.Family = MyUser.Family;
            user.PhoneNumber = MyUser.PhoneNumber;
            user.UserName = MyUser.UserName;
            user.NormalizedUserName = MyUser.UserName.ToUpper();

            if (Input.SelectedRole != MyUser.InRole)
            {
                await _userManager.RemoveFromRoleAsync(user, MyUser.InRole);
                await _userManager.AddToRoleAsync(user, Input.SelectedRole);
                user.InRole = Input.SelectedRole;
            }

            if (Input.Password != null)
            {
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, Input.Password);
            }

            if (imgUp != null)
            {
                string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img/ProfileImages", user.ProfileImage);
                if (System.IO.File.Exists(deletePath))
                    System.IO.File.Delete(deletePath);


                string SaveDir = "wwwroot/Img/ProfileImages";
                if (!Directory.Exists(SaveDir))
                    Directory.CreateDirectory(SaveDir);

                user.ProfileImage = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                string savePath = Path.Combine(Directory.GetCurrentDirectory(), SaveDir, user.ProfileImage);
                using (var FileSream = new FileStream(savePath, FileMode.Create))
                {
                    imgUp.CopyTo(FileSream);
                }
            }

            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToPage("./index");
        }

        void initRoles()
        {
            ViewData["Inrole"] = new SelectList(_roleManager.Roles, "Name", "Name", MyUser.InRole);
        }

    }
}
