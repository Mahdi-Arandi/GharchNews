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
using System.Security.Claims;

namespace GharchNews.Pages.Admin.User
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ProfileModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IFormFile? imgUp { get; set; }

        [BindProperty]
        public Users MyUser { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId= User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return NotFound();
            }

            MyUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (MyUser == null)
            {
                return NotFound();
            }

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == MyUser.Id);
            if (user == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            user.Name = MyUser.Name;
            user.Family = MyUser.Family;
            user.PhoneNumber = MyUser.PhoneNumber;
            user.ProfileImage = MyUser.ProfileImage;

            if (imgUp != null)
            {
                if (MyUser.ProfileImage != null)
                {
                    string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img/ProfileImages", user.ProfileImage);
                    if (System.IO.File.Exists(deletePath))
                        System.IO.File.Delete(deletePath);
                }

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
            return RedirectToPage("../index");
        }

        public IActionResult OnGetDeleteProfile(string id)
        {
            var MyUser = _context.Users.FirstOrDefault(r => r.Id == id);

            MyUser.ProfileImage = null;
            _context.SaveChanges();
            return RedirectToAction("./Profile");
        }
    }
}
