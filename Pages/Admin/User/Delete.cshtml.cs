using GharchNews.Data;
using GharchNews.Models;
using GharchNews.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GharchNews.Pages.Admin.User
{
    [Authorize(Roles = SD.Admin)]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Users MyUser { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MyUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (MyUser == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MyUser = await _context.Users.FindAsync(id);

            if (MyUser != null)
            {
                if (MyUser.ProfileImage != null)
                {
                    string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img/ProfileImages", MyUser.ProfileImage);
                    if (System.IO.File.Exists(deletePath))
                        System.IO.File.Delete(deletePath);
                }
                _context.Remove(MyUser);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("./index");
        }

    }
}
