using GharchNews.Data;
using GharchNews.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections;

namespace GharchNews.Pages
{
    public class GalleryModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public GalleryModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Gallery> Galleries { get; set; }
        public void OnGet()
        {
            Galleries = _context.Galleries.Where(g => g.IsSuccess == true).ToList();
        }
    }
}
