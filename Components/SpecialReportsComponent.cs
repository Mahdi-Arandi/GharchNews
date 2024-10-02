using GharchNews.Data;
using GharchNews.Models;
using GharchNews.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GharchNews.Components
{
    public class SpecialReportsComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public SpecialReportsComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Report> Reports { get; set; }

        public async Task<IViewComponentResult> InvokeAsync(string groupName = ReportGroups.MultiMedia, int take = 4, int skip = 1)
        {
            Reports = await _context.Reports.Where(r => r.ReportGroup.GroupName != groupName).OrderByDescending(r => r.CreateDate).Skip(skip).Take(take).ToListAsync();
            return View("/Pages/Components/_SpecialReports.cshtml", Reports);
        }
    }
}
