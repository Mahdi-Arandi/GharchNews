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
using Microsoft.AspNetCore.Authorization;
using GharchNews.Roles;

namespace GharchNews.Pages.Admin.Settings.AboutUs
{
    [Authorize(Roles = SD.Admin)]
    public class EditModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public EditModel(GharchNews.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.AboutUs AboutUs { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AboutUs = await _context.AboutUs.FirstOrDefaultAsync(m => m.AboutId == id);
            if (AboutUs == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(AboutUs).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AboutUsExists(AboutUs.AboutId))
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

        private bool AboutUsExists(int id)
        {
            return _context.AboutUs.Any(e => e.AboutId == id);
        }
    }
}
