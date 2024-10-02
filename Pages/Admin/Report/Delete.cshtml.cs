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
    public class DeleteModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public DeleteModel(GharchNews.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Report Report { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Report = await _context.Reports.Include(g=>g.ReportGroup).FirstOrDefaultAsync(m => m.ReportId == id);

            if (Report == null)
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

            Report = await _context.Reports.FindAsync(id);
            if (Report != null)
            {
                if (Report.Image != null)
                {
                    string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img/ReportImages", Report.Image);
                    if (System.IO.File.Exists(deletePath))
                        System.IO.File.Delete(deletePath);
                }

                _context.Reports.Remove(Report);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
