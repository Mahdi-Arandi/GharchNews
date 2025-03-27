using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GharchNews.Data;
using GharchNews.Models;
using Microsoft.AspNetCore.Authorization;
using GharchNews.Roles;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GharchNews.Pages.Admin.Image
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public DeleteModel(GharchNews.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Image Image { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            initImage(id);
            if (Image == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole(SD.Admin))
            {
                Image = await _context.Images.Include(g => g.Gallery).FirstOrDefaultAsync(m => m.ImageId == id);
            }
            else if (User.IsInRole(SD.User) && Image.Id == userId && Image.IsSuccess == false)
            {
                Image = await _context.Images.Include(g => g.Gallery).FirstOrDefaultAsync(m => m.ImageId == id);
            }
            else
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Image = await _context.Images.FindAsync(id);
            if (Image != null)
            {
                _context.Images.Remove(Image);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        void initImage(int? id)
        {
            Image = _context.Images.Include(g => g.Gallery).FirstOrDefault(m => m.ImageId == id);

            ViewData["GalleryId"] = new SelectList(_context.Galleries, "GalleryId", "GalleryName");
        }
    }
}
