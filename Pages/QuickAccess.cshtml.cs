using GharchNews.Data;
using GharchNews.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GharchNews.Pages
{
    public class QuickAccessModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public QuickAccessModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ReportGroup> ReportGroups { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            ReportGroups = await _context.ReportGroups.ToListAsync();
            return Page();
        }
    }
}
