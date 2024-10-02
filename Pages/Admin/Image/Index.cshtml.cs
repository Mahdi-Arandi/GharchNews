using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GharchNews.Data;
using GharchNews.Models;

namespace GharchNews.Pages.Admin.Image
{
    public class IndexModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public IndexModel(GharchNews.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.Image> Images { get; set; } = default!;
        public Models.Image Image { get; set; } = default!;

        public async Task OnGetAsync()
        {
            ViewData["Galleries"] = await _context.Galleries.Select(g => g.GalleryName).ToListAsync();

            Images = await _context.Images
                .Include(i => i.Gallery).ToListAsync();
        }

        public async Task OnGetGallery(string GalleryName)
        {
            ViewData["CurrentGallery"] = GalleryName;

            Images = await _context.Images
                .Include(i => i.Gallery)
                .Where(i => i.Gallery.GalleryName == GalleryName)
                .OrderByDescending(i => i.CreateDate).ToListAsync();
        }

        public async Task<IActionResult> OnGetDelete(int? id, string? ReturnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            Image = await _context.Images.FindAsync(id);

            var galleryName = await _context.Images
                .Where(i => i.ImageId == id)
                .Select(g => g.Gallery.GalleryName)
                .FirstOrDefaultAsync();

            if (Image != null)
            {
                if (Image.ImageName != null)
                {
                    string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img/GalleryImages/" + galleryName, Image.ImageName);
                    if (System.IO.File.Exists(deletePath))
                        System.IO.File.Delete(deletePath);
                }

                _context.Images.Remove(Image);
                await _context.SaveChangesAsync();
            }

            if (ReturnUrl == null)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                return LocalRedirect("~/Admin/Image?GalleryName=" + ReturnUrl + "&handler=Gallery");
            }

        }

    }
}
