using GharchNews.Data;
using GharchNews.Models;
using GharchNews.Models.ViewModels;
using GharchNews.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GharchNews.Components
{
    public class SideMenuComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public SideMenuComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public ImageAndReportGroupVM ImageAndReportGroupVM { get; set; }
        public async Task<IViewComponentResult> InvokeAsync(int take = 8)
        {
            var galleries = await _context.Galleries.Select(g => g.GalleryName).ToListAsync();
            int itemCount = _context.Galleries.Count();
            int index = new Random().Next(0, itemCount);
            var galleryName = galleries[index];
            ViewData["galleryName"] = galleryName;

            ImageAndReportGroupVM = new ImageAndReportGroupVM()
            {
                Images = await _context.Images.Include(g => g.Gallery).Where(g => g.Gallery.GalleryName == galleryName && g.IsSuccess == true).Take(take).ToListAsync(),
                ReportGroups = await _context.ReportGroups.Select(g => g.GroupName).ToListAsync()
            };
            return View("/Pages/Components/_SideMenu.cshtml", ImageAndReportGroupVM);
        }
    }
}
