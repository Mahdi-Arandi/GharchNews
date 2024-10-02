using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GharchNews.Data;
using GharchNews.Models;

namespace GharchNews.Pages.Admin.Gallery
{
    public class DeleteModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public DeleteModel(GharchNews.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Gallery Gallery { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Gallery = await _context.Galleries.FirstOrDefaultAsync(m => m.GalleryId == id);

            if (Gallery == null)
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

            Gallery = await _context.Galleries.FindAsync(id);
            if (Gallery != null)
            {
                _context.Galleries.Remove(Gallery);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
