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

namespace GharchNews.Pages.Admin.ReportGroup
{
    public class EditModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public EditModel(GharchNews.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.ReportGroup ReportGroup { get; set; } = default!;
        public IFormFile? imgUp { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ReportGroup = await _context.ReportGroups.FirstOrDefaultAsync(m => m.GroupId == id);
            if (ReportGroup == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (imgUp != null)
            {
                if (ReportGroup.GroupImage != null)
                {
                    string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img/ReportGroupImages", ReportGroup.GroupImage);
                    if (System.IO.File.Exists(deletePath))
                        System.IO.File.Delete(deletePath);
                }

                string SaveDir = "wwwroot/Img/reportGroupImages";
                if (!Directory.Exists(SaveDir))
                    Directory.CreateDirectory(SaveDir);

                ReportGroup.GroupImage = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                string savePath = Path.Combine(Directory.GetCurrentDirectory(), SaveDir, ReportGroup.GroupImage);
                using (var FileSream = new FileStream(savePath, FileMode.Create))
                {
                    imgUp.CopyTo(FileSream);
                }
            }

            _context.Attach(ReportGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReportGroupExists(ReportGroup.GroupId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ReportGroupExists(int id)
        {
            return _context.ReportGroups.Any(e => e.GroupId == id);
        }
    }
}
