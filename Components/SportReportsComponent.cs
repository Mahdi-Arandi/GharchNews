using GharchNews.Data;
using GharchNews.Models;
using GharchNews.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GharchNews.Components
{
    public class SportReportsComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public SportReportsComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Report> Reports { get; set; }

        public async Task<IViewComponentResult> InvokeAsync(string groupName=ReportGroups.Sport , int take=4)
        {
            Reports= await _context.Reports.Where(r=>r.ReportGroup.GroupName== groupName).OrderByDescending(r=>r.CreateDate).Take(take).ToListAsync();
            return View("/Pages/Components/_SportReports.cshtml", Reports);
        }
    }
}
