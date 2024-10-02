using GharchNews.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GharchNews.Pages.Admin.ReportGroup
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.ReportGroup> ReportGroup { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ReportGroup = await _context.ReportGroups.ToListAsync();
            return Page();
        }
    }
}
