using GharchNews.Data;
using GharchNews.Models;
using GharchNews.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GharchNews.Components
{
    public class SideGalleryComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public SideGalleryComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Gallery> Galleries { get; set; } = default!;
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Galleries = await _context.Galleries.ToListAsync();
            return View("/Pages/Components/_SideGallery.cshtml", Galleries);
        }
    }
}
