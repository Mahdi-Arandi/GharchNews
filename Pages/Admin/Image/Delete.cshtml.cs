using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GharchNews.Data;
using GharchNews.Models;

namespace GharchNews.Pages.Admin.Image
{
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

            Image = await _context.Images.FirstOrDefaultAsync(m => m.ImageId == id);

            if (Image == null)
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
    }
}
