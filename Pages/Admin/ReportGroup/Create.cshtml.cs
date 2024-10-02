using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GharchNews.Data;
using GharchNews.Models;

namespace GharchNews.Pages.Admin.ReportGroup
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
            return Page();
        }

        [BindProperty]
        public Models.ReportGroup ReportGroup { get; set; } = default!;

        public IFormFile? imgUp { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (imgUp != null)
            {
                string SaveDir = "wwwroot/Img/reportGroupImages";
                if(!Directory.Exists(SaveDir))
                    Directory.CreateDirectory(SaveDir);

                ReportGroup.GroupImage = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                string savePath= Path.Combine(Directory.GetCurrentDirectory(), SaveDir, ReportGroup.GroupImage);
                using(var FileSream= new FileStream(savePath, FileMode.Create))
                {
                    imgUp.CopyTo(FileSream);
                }
            }

            _context.ReportGroups.Add(ReportGroup);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
