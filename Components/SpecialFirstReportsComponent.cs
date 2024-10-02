using GharchNews.Data;
using GharchNews.Models;
using GharchNews.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GharchNews.Components
{
    public class SpecialFirstReportsComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public SpecialFirstReportsComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public Report Report { get; set; }

        public async Task<IViewComponentResult> InvokeAsync(string groupName = ReportGroups.MultiMedia)
        {
            Report = await _context.Reports.Where(r => r.ReportGroup.GroupName != groupName).OrderByDescending(r => r.CreateDate).FirstAsync();
            return View("/Pages/Components/_SpecialFirstReports.cshtml", Report);
        }
    }
}
