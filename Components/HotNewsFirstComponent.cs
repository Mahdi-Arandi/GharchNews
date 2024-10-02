using GharchNews.Data;
using GharchNews.Models;
using GharchNews.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GharchNews.Components
{
    public class HotNewsFirstComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public HotNewsFirstComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public Report Report { get; set; }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            Report = await _context.Reports.Include(r=>r.ReportGroup).Where(r => r.IsHotNews == true).OrderByDescending(r => r.HotNewsDate).FirstAsync();
            return View("/Pages/Components/_HotNewsFirst.cshtml", Report);
        }
    }
}
