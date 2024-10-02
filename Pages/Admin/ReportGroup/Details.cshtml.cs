using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GharchNews.Data;
using GharchNews.Models;

namespace GharchNews.Pages.Admin.ReportGroup
{
    public class DetailsModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public DetailsModel(GharchNews.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Models.ReportGroup ReportGroup { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ReportGroup = await _context.ReportGroups.FirstOrDefaultAsync(m => m.GroupId == id);
            if (ReportGroup == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
