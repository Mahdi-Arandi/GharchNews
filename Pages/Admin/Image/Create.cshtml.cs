using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GharchNews.Data;
using GharchNews.Models;
using Microsoft.EntityFrameworkCore;

namespace GharchNews.Pages.Admin.Image
{
    public class CreateModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public CreateModel(GharchNews.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["GalleryId"] = new SelectList(_context.Galleries, "GalleryId", "GalleryName");
            return Page();
        }

        [BindProperty]
        public Models.Image Image { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD

        public List<IFormFile> imgUp { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync(int GalleryId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var GalleryName = await _context.Galleries
                .Where(g => g.GalleryId == GalleryId)
                .Select(g => g.GalleryName).FirstOrDefaultAsync();

            if (imgUp != null)
            {
                string SaveDir = "wwwroot/Img/galleryImages/" + GalleryName;
                if (!Directory.Exists(SaveDir))
                    Directory.CreateDirectory(SaveDir);

                foreach (var file in imgUp)
                {
                    string img = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string savePath = Path.Combine(Directory.GetCurrentDirectory(), SaveDir, img);
                    using (var FileSream = new FileStream(savePath, FileMode.Create))
                    {
                        file.CopyTo(FileSream);
                    }

                    Image = new Models.Image
                    {
                        ImageName = img,
                        CreateDate = DateTime.Now,
                        GalleryId = GalleryId,
                        Gallery = Image.Gallery
                    };

                    _context.Images.Add(Image);

                }
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
