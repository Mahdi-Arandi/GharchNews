using GharchNews.Data;
using GharchNews.Models;
using GharchNews.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GharchNews.Components
{
    public class AdsComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public AdsComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Ads> Ads { get; set; } = default!;
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Ads = await _context.Ads.Where(a => a.ExpireDate > DateTime.Now).ToListAsync();
            return View("/Pages/Components/_Ads.cshtml", Ads);
        }
    }
}
