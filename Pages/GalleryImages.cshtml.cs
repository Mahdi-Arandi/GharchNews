using GharchNews.Data;
using GharchNews.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GharchNews.Pages
{
    public class GalleryImagesModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public GalleryImagesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Image> Images { get; set; }
        public void OnGet(int id, string GalleryName)
        {
            Images = _context.Images.Include(g => g.Gallery).Where(g => g.GalleryId == id && g.IsSuccess == true).ToList();
            ViewData["GalleryName"] = GalleryName;
        }


    }
}
