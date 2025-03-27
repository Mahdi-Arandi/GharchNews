using GharchNews.Data;
using GharchNews.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GharchNews.Pages
{
    public class SearchModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public SearchModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Report> Reports { get; set; }
        public async Task<IActionResult> OnGetAsync(string q, int pageId = 1)
        {
            int take = 16;
            int skip = (pageId - 1) * take;
            int itemCount = _context.Reports.Where(r => r.title.Contains(q) ||
                r.Description.Contains(q) ||
                r.FullText.Contains(q))
                .Where(r => r.IsSuccess == true)
                .Count();
            ViewData["itemCount"] = itemCount;
            ViewData["take"] = take;
            ViewData["pageId"] = pageId;
            ViewData["previousPage"] = pageId - 1;
            ViewData["nextPage"] = pageId + 1;
            if (itemCount % take == 0)
            {
                ViewData["pageCount"] = (itemCount / take);
            }
            else
            {
                ViewData["pageCount"] = (itemCount / take) + 1;
            }

            ViewData["Searched"] = q;

            Reports = await _context.Reports.Include(r => r.ReportGroup)
                .Where(r => r.title.Contains(q) ||
                r.Description.Contains(q) ||
                r.FullText.Contains(q))
                .Where(r => r.IsSuccess == true)
                .Distinct()
                .OrderByDescending(r => r.CreateDate).Skip(skip).Take(take).ToListAsync();
            return Page();
        }
    }
}
