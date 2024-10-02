using GharchNews.Data;
using GharchNews.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GharchNews.Pages
{
    public class ReportModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ReportModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public Report Report { get; set; }
        public IEnumerable<Report> Reports { get; set; }
        public void OnGet(int id)
        {
            Report = _context.Reports.Include(r => r.ReportGroup).Where(r => r.ReportId == id).FirstOrDefault();
            Report.View += 1;
            _context.Update(Report);
            _context.SaveChanges();

            int take = 10;
            string groupName = Report.ReportGroup.GroupName;
            Reports = _context.Reports.Where(r => r.ReportGroup.GroupName == groupName && r.ReportId != id).OrderByDescending(r => r.View).Take(take).ToList();
        }
    }
}
