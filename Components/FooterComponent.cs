using GharchNews.Data;
using GharchNews.Models;
using GharchNews.Models.ViewModels;
using GharchNews.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GharchNews.Components
{
    public class FooterComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public FooterComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public FooterVM FooterVM { get; set; }

        public async Task<IViewComponentResult> InvokeAsync(int commentAndreportTake = 4, int ImageTake = 10, string groupName = ReportGroups.MultiMedia)
        {
            var galleries = await _context.Galleries.Select(g => g.GalleryName).ToListAsync();
            int itemCount = _context.Galleries.Count();
            int index = new Random().Next(0, itemCount);
            var galleryName = galleries[index];
            ViewData["galleryName"] = galleryName;

            FooterVM = new FooterVM()
            {
                Reports = await _context.Reports.Where(r => r.ReportGroup.GroupName != groupName && r.IsSuccess == true).OrderByDescending(r => r.CreateDate).Take(commentAndreportTake).ToListAsync(),
                Comments = await _context.Comments.Where(c => c.IsSuccess == true).OrderByDescending(c => c.CreateDate).Take(commentAndreportTake).ToListAsync(),
                Images = await _context.Images.Include(g => g.Gallery).Where(g => g.Gallery.GalleryName == galleryName && g.IsSuccess == true).Take(ImageTake).ToListAsync(),
                AboutUs = await _context.AboutUs.FirstOrDefaultAsync()
            };
            return View("/Pages/Components/_Footer.cshtml", FooterVM);
        }
    }
}
