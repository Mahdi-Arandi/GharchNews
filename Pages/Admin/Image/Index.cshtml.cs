using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GharchNews.Data;
using GharchNews.Models;
using GharchNews.Roles;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GharchNews.Pages.Admin.Image
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public IndexModel(GharchNews.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.Image> Images { get; set; } = default!;
        public Models.Image Image { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? returnUrl)
        {
            ViewData["Galleries"] = await _context.Galleries.Select(g => g.GalleryName).ToListAsync();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole(SD.Admin))
            {
                Images = await _context.Images
                    .Include(i => i.Gallery)
                    .Include(i => i.Users)
                    .OrderByDescending(i => i.CreateDate)
                    .ToListAsync();
            }
            else
            {
                Images = await _context.Images
                    .Include(i => i.Gallery)
                    .Include(i => i.Users)
                    .Where(i => i.Id == userId)
                    .OrderByDescending(i => i.CreateDate)
                    .ToListAsync();
            }


            if (returnUrl == null)
            {
                return Page();
            }
            else
            {
                return LocalRedirect("~/Admin/Image?GalleryName=" + returnUrl + "&handler=Gallery");
            }

        }

        public async Task OnGetGallery(string GalleryName)
        {
            ViewData["CurrentGallery"] = GalleryName;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole(SD.Admin))
            {
                Images = await _context.Images
                    .Include(i => i.Gallery)
                    .Include(i => i.Users)
                    .Where(i => i.Gallery.GalleryName == GalleryName)
                    .OrderByDescending(i => i.CreateDate).ToListAsync();
            }
            else
            {
                Images = await _context.Images
                    .Include(i => i.Gallery)
                    .Include(i => i.Users)
                    .Where(i => i.Id == userId)
                    .Where(i => i.Gallery.GalleryName == GalleryName)
                    .OrderByDescending(i => i.CreateDate).ToListAsync();
            }

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

        public IActionResult OnGetIsSuccess(int id)
        {
            var MyImage = _context.Images.FirstOrDefault(i => i.ImageId == id);

            if (MyImage.IsSuccess == true)
            {
                MyImage.IsSuccess = false;
                _context.SaveChanges();
                return RedirectToAction("./index");
            }

            MyImage.IsSuccess = true;
            _context.SaveChanges();
            return RedirectToAction("./index");
        }

    }
}
