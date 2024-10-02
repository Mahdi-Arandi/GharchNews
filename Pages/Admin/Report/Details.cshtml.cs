using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GharchNews.Data;
using GharchNews.Models;

namespace GharchNews.Pages.Admin.Report
{
    public class DetailsModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public DetailsModel(GharchNews.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Models.Report Report { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Report = await _context.Reports.Include(g => g.ReportGroup).FirstOrDefaultAsync(m => m.ReportId == id);
            if (Report == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
