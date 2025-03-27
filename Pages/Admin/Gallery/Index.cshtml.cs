using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GharchNews.Data;
using GharchNews.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GharchNews.Roles;

namespace GharchNews.Pages.Admin.Gallery
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public IndexModel(GharchNews.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.Gallery> Galleries { get; set; } = default!;
        public Models.Gallery Gallery { get; set; }

        public IList<Models.Image> Image { get; set; }

        public async Task OnGetAsync()
        {
            ViewData["userId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole(SD.Admin))
            {
                Galleries = await _context.Galleries.Include(g => g.Users).ToListAsync();
            }
            else
            {
                Galleries = await _context.Galleries.Where(g => g.Id == userId).Include(g => g.Users).ToListAsync();
            }

            Image = await _context.Images.ToListAsync();
        }


        public async Task<IActionResult> OnGetDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Gallery = await _context.Galleries.FindAsync(id);
            if (Gallery != null)
            {
                _context.Galleries.Remove(Gallery);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        public IActionResult OnGetIsSuccess(int id)
        {
            var MyGallery = _context.Galleries.FirstOrDefault(i => i.GalleryId == id);

            if (MyGallery.IsSuccess == true)
            {
                MyGallery.IsSuccess = false;
                _context.SaveChanges();
                return RedirectToAction("./index");
            }

            MyGallery.IsSuccess = true;
            _context.SaveChanges();
            return RedirectToAction("./index");
        }

    }
}
