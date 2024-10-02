using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GharchNews.Data;
using GharchNews.Models;

namespace GharchNews.Pages.Admin.Report
{
    public class EditModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public EditModel(GharchNews.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Report Report { get; set; } = default!;

        public IFormFile? imgUp { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Report =  await _context.Reports.Include(g=>g.ReportGroup).FirstOrDefaultAsync(m => m.ReportId == id);
            if (Report == null)
            {
                return NotFound();
            }
           ViewData["GroupId"] = new SelectList(_context.ReportGroups, "GroupId", "GroupName");
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
                if (Report.Image != null)
                {
                    string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img/reportImages", Report.Image);
                    if (System.IO.File.Exists(deletePath))
                        System.IO.File.Delete(deletePath);
                }

                string SaveDir = "wwwroot/Img/reportImages";
                if (!Directory.Exists(SaveDir))
                    Directory.CreateDirectory(SaveDir);

                Report.Image = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                string savePath = Path.Combine(Directory.GetCurrentDirectory(), SaveDir, Report.Image);
                using (var FileSream = new FileStream(savePath, FileMode.Create))
                {
                    imgUp.CopyTo(FileSream);
                }
            }

            _context.Attach(Report).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReportExists(Report.ReportId))
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

        private bool ReportExists(int id)
        {
            return _context.Reports.Any(e => e.ReportId == id);
        }
    }
}
