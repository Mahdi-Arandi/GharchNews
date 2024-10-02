using GharchNews.Data;
using GharchNews.Models;
using GharchNews.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GharchNews.Pages
{
    public class GroupModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public GroupModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Report> Reports { get; set; }

        public async Task<IActionResult> OnGetAsync(string group, int pageId = 1)
        {
            int take = 16;
            int skip = (pageId - 1) * take;
            int itemCount = _context.Reports.Include(r => r.ReportGroup).Where(r => r.ReportGroup.GroupName == group).Count();
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

            if (group == ReportGroups.HotNews)
            {
                Reports=await _context.Reports.Include(r=>r.ReportGroup).Where(r=>r.IsHotNews==true).OrderByDescending(r=>r.HotNewsDate)
                    .Skip(skip).Take(take).ToListAsync();
                ViewData["GroupName"] = group;
                return Page();
            }

            Reports = await _context.Reports.Include(r => r.ReportGroup).Where(r => r.ReportGroup.GroupName == group)
                .OrderByDescending(r => r.CreateDate).Skip(skip).Take(take).ToListAsync();
            ViewData["GroupName"] = group;
            return Page();
        }
    }
}
