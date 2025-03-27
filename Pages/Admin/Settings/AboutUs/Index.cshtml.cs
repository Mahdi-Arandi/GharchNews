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

namespace GharchNews.Pages.Admin.Settings.AboutUs
{
    [Authorize(Roles = SD.Admin)]
    public class IndexModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public IndexModel(GharchNews.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.AboutUs> AboutUs { get;set; } = default!;

        public async Task OnGetAsync()
        {
            AboutUs = await _context.AboutUs.ToListAsync();
        }

        public async Task<IActionResult> OnGetDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutUs = await _context.AboutUs.FirstAsync(a => a.AboutId == id);
            if (aboutUs != null)
            {
                _context.AboutUs.Remove(aboutUs);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("index");
        }
    }
}
