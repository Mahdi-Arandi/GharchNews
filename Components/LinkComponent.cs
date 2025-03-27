using GharchNews.Data;
using GharchNews.Models;
using GharchNews.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GharchNews.Components
{
    public class LinkComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public LinkComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Links> Links { get; set; } = default!;
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Links = await _context.Links.Where(l => l.IsSuccess == true).ToListAsync();
            return View("/Pages/Components/_Link.cshtml", Links);
        }
    }
}
