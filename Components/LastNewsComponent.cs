using GharchNews.Data;
using GharchNews.Models;
using GharchNews.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GharchNews.Components
{
    public class LastNewsComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public LastNewsComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Report> Reports { get; set; }

        public async Task<IViewComponentResult> InvokeAsync(int take = 15)
        {
            Reports = await _context.Reports.Include(r => r.ReportGroup).Where(r => r.IsSuccess == true).OrderByDescending(r => r.CreateDate).Take(take).ToListAsync();
            return View("/Pages/Components/_LastNews.cshtml", Reports);
        }
    }
}
