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
using GharchNews.Roles;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IO;

namespace GharchNews.Pages.Admin.Report
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
        public Models.Report Report { get; set; } = default!;

        public IFormFile? imgUp { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, string? ReturnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            initReport(id);

            if (Report == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole(SD.Admin))
            {
                Report = await _context.Reports.Include(g => g.ReportGroup).FirstOrDefaultAsync(m => m.ReportId == id);
            }
            else if (User.IsInRole(SD.User) && Report.Id == userId && Report.IsSuccess == false)
            {
                Report = await _context.Reports.Include(g => g.ReportGroup).FirstOrDefaultAsync(m => m.ReportId == id);
            }
            else
            {
                return NotFound();
            }

            ViewData["ReturnUrl"] = ReturnUrl ?? "Index";
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string? ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var reportToUpdate = await _context.Reports.FindAsync(id);
            if (reportToUpdate == null)
            {
                return NotFound();
            }

            reportToUpdate.title = Report.title;
            reportToUpdate.Description = Report.Description;
            reportToUpdate.FullText = Report.FullText;
            reportToUpdate.IsDraft = false; // هنگام ویرایش، خبر نهایی می‌شود
            reportToUpdate.IsSuccess = true; // خبر منتشر می‌شود

            var isHotNews = _context.Reports.Where(r => r.ReportId == id).Select(r => r.IsHotNews).FirstOrDefault();
            if (Report.IsHotNews != isHotNews && Report.IsHotNews == true)
            {
                reportToUpdate.HotNewsDate = DateTime.Now;
            }

            if (imgUp != null)
            {
                if (reportToUpdate.Image != null)
                {
                    string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img/reportImages", reportToUpdate.Image);
                    if (System.IO.File.Exists(deletePath))
                        System.IO.File.Delete(deletePath);
                }

                string SaveDir = "wwwroot/Img/reportImages";
                if (!Directory.Exists(SaveDir))
                    Directory.CreateDirectory(SaveDir);

                reportToUpdate.Image = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                string savePath = Path.Combine(Directory.GetCurrentDirectory(), SaveDir, reportToUpdate.Image);
                using (var FileStream = new FileStream(savePath, FileMode.Create))
                {
                    imgUp.CopyTo(FileStream);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReportExists(reportToUpdate.ReportId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage(ReturnUrl ?? "./Index");
        }

        public async Task<IActionResult> OnPostSaveDraftAsync(int? id, string? ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var reportToUpdate = await _context.Reports.FindAsync(id);
            if (reportToUpdate == null)
            {
                return NotFound();
            }

            reportToUpdate.title = Report.title;
            reportToUpdate.Description = Report.Description;
            reportToUpdate.FullText = Report.FullText;
            reportToUpdate.IsDraft = true; // ذخیره به‌عنوان پیش‌نویس
            reportToUpdate.IsSuccess = false; // هنوز منتشر نشده

            if (imgUp != null)
            {
                if (reportToUpdate.Image != null)
                {
                    string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img/reportImages", reportToUpdate.Image);
                    if (System.IO.File.Exists(deletePath))
                        System.IO.File.Delete(deletePath);
                }

                string SaveDir = "wwwroot/Img/reportImages";
                if (!Directory.Exists(SaveDir))
                    Directory.CreateDirectory(SaveDir);

                reportToUpdate.Image = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                string savePath = Path.Combine(Directory.GetCurrentDirectory(), SaveDir, reportToUpdate.Image);
                using (var FileStream = new FileStream(savePath, FileMode.Create))
                {
                    imgUp.CopyTo(FileStream);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReportExists(reportToUpdate.ReportId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage(ReturnUrl ?? "./Index");
        }

        void initReport(int? id)
        {
            Report = _context.Reports.Include(g => g.ReportGroup).FirstOrDefault(m => m.ReportId == id);
            ViewData["GroupId"] = new SelectList(_context.ReportGroups, "GroupId", "GroupName");
        }

        private bool ReportExists(int id)
        {
            return _context.Reports.Any(e => e.ReportId == id);
        }
    }
}
