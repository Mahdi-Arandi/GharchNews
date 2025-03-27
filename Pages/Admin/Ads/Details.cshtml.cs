using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GharchNews.Data;
using GharchNews.Models;
using GharchNews.Roles;
using Microsoft.AspNetCore.Authorization;

namespace GharchNews.Pages.Admin.Ads
{
    [Authorize(Roles = SD.Admin)]
    public class DetailsModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public DetailsModel(GharchNews.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Models.Ads Ads { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ads = await _context.Ads.FirstOrDefaultAsync(m => m.AdsId == id);
            if (Ads == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
