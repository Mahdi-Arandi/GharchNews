using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GharchNews.Data;
using GharchNews.Models;
using Microsoft.AspNetCore.Authorization;
using GharchNews.Roles;

namespace GharchNews.Pages.Admin.Ads
{
    [Authorize(Roles = SD.Admin)]
    public class CreateModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public CreateModel(GharchNews.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Models.Ads Ads { get; set; } = default!;
        public IFormFile imgUp { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Ads.CreateDate = DateTime.Now;

            if (imgUp != null)
            {
                string SaveDir = "wwwroot/Img/AdsImages";
                if (!Directory.Exists(SaveDir))
                    Directory.CreateDirectory(SaveDir);

                Ads.Image = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                string savePath = Path.Combine(Directory.GetCurrentDirectory(), SaveDir, Ads.Image);
                using (var FileSream = new FileStream(savePath, FileMode.Create))
                {
                    imgUp.CopyTo(FileSream);
                }
            }

            _context.Ads.Add(Ads);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
