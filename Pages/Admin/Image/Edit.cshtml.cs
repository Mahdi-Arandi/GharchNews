using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GharchNews.Data;
using GharchNews.Models;

namespace GharchNews.Pages.Admin.Image
{
    public class EditModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public EditModel(GharchNews.Data.ApplicationDbContext context)
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

            Image =  await _context.Images.FirstOrDefaultAsync(m => m.ImageId == id);
            if (Image == null)
            {
                return NotFound();
            }
           ViewData["GalleryId"] = new SelectList(_context.Galleries, "GalleryId", "GalleryName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Image).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageExists(Image.ImageId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ImageExists(int id)
        {
            return _context.Images.Any(e => e.ImageId == id);
        }
    }
}
