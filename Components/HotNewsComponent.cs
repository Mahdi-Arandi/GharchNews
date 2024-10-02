using GharchNews.Data;
using GharchNews.Models;
using GharchNews.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GharchNews.Components
{
    public class HotNewsComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public HotNewsComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Report> Reports { get; set; }

        public async Task<IViewComponentResult> InvokeAsync(int take = 9, int skip = 1)
        {
            Reports = await _context.Reports.Include(r=>r.ReportGroup).Where(r => r.IsHotNews == true).OrderByDescending(r=>r.HotNewsDate).Skip(skip).Take(take).ToListAsync();
            return View("/Pages/Components/_HotNews.cshtml", Reports);
        }
    }
}
