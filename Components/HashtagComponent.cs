using GharchNews.Data;
using GharchNews.Models;
using GharchNews.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GharchNews.Components
{
    public class HashtagComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public HashtagComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Hashtag> Hashtags { get; set; }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            Hashtags = await _context.Hashtags.ToListAsync();
            return View("/Pages/Components/_Hashtag.cshtml", Hashtags);
        }
    }
}
