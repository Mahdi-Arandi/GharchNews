using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GharchNews.Data;
using GharchNews.Models;
using GharchNews.Roles;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace GharchNews.Pages.Admin.Image
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public EditModel(GharchNews.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Image Image { get; set; } = default!;
        public IFormFile? imgUp { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, string? returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            initImage(id);
            if (Image == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole(SD.Admin))
            {
                Image = await _context.Images.Include(g => g.Gallery).FirstOrDefaultAsync(m => m.ImageId == id);
            }
            else if (User.IsInRole(SD.User) && Image.Id == userId && Image.IsSuccess == false)
            {
                Image = await _context.Images.Include(g => g.Gallery).FirstOrDefaultAsync(m => m.ImageId == id);
            }
            else
            {
                return NotFound();
            }


            ViewData["returnUrl"] = returnUrl;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? id, string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            var galleryName = await _context.Images
                    .Where(i => i.ImageId == id)
                    .Select(g => g.Gallery.GalleryName)
                    .FirstOrDefaultAsync();

            var newGalleryName = await _context.Galleries
                .Where(g => g.GalleryId == Image.GalleryId)
                .Select(g => g.GalleryName)
                .FirstOrDefaultAsync();

            if (imgUp != null)
            {
                string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img/GalleryImages/" + galleryName, Image.ImageName);
                if (System.IO.File.Exists(deletePath))
                    System.IO.File.Delete(deletePath);

                string SaveDir = "wwwroot/Img/galleryImages/" + galleryName;
                if (!Directory.Exists(SaveDir))
                    Directory.CreateDirectory(SaveDir);

                Image.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                string savePath = Path.Combine(Directory.GetCurrentDirectory(), SaveDir, Image.ImageName);
                using (var FileSream = new FileStream(savePath, FileMode.Create))
                {
                    imgUp.CopyTo(FileSream);
                }

                _context.Images.Add(Image);

            }

            if (galleryName != newGalleryName)
            {
                string lastSaveDir = "wwwroot/Img/galleryImages/" + galleryName;
                string lastSavePath = Path.Combine(Directory.GetCurrentDirectory(), lastSaveDir, Image.ImageName);
                string newSaveDir = "wwwroot/Img/galleryImages/" + newGalleryName;
                if (!Directory.Exists(newSaveDir))
                    Directory.CreateDirectory(newSaveDir);
                string newSavePath = Path.Combine(Directory.GetCurrentDirectory(), newSaveDir, Image.ImageName);
                System.IO.File.Move(lastSavePath, newSavePath, true);
            }

            _context.Attach(Image).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageExists(Image.ImageId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            if (returnUrl == null)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                return LocalRedirect("~/Admin/Image?GalleryName=" + returnUrl + "&handler=Gallery");
            }
        }

        void initImage(int? id)
        {
            Image = _context.Images.Include(g => g.Gallery).FirstOrDefault(m => m.ImageId == id);

            ViewData["GalleryId"] = new SelectList(_context.Galleries.Where(i => i.IsSuccess == true), "GalleryId", "GalleryName");
        }

        private bool ImageExists(int id)
        {
            return _context.Images.Any(e => e.ImageId == id);
        }
    }
}
