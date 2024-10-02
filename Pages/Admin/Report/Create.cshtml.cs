using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GharchNews.Data;
using GharchNews.Models;

namespace GharchNews.Pages.Admin.Report
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
            initReportGroup();
            return Page();
        }

        [BindProperty]
        public Models.Report Report { get; set; } = default!;

        public IFormFile? imgUp { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                initReportGroup();
                return Page();
            }

            Report.CreateDate = DateTime.Now;
            Report.View = 0;

            if (imgUp != null)
            {
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

            _context.Reports.Add(Report);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        void initReportGroup()
        {
            ViewData["GroupId"] = new SelectList(_context.ReportGroups, "GroupId", "GroupName");
        }

    }
}
