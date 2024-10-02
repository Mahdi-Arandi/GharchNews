using GharchNews.Data;
using GharchNews.Models;
using GharchNews.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GharchNews.Components
{
    public class MultimediaSliderComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public MultimediaSliderComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Report> Reports { get; set; }

        public async Task<IViewComponentResult> InvokeAsync(string groupName = ReportGroups.MultiMedia, int take = 5)
        {
            Reports = await _context.Reports.Where(r => r.ReportGroup.GroupName == groupName).OrderByDescending(r => r.CreateDate).Take(take).ToListAsync();
            return View("/Pages/Components/_MultiMediaSlider.cshtml", Reports);
        }
    }
}
