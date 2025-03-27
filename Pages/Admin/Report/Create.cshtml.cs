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
using System.Security.Claims;
using GharchNews.Roles;

namespace GharchNews.Pages.Admin.Report
{
    [Authorize]
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

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var myUser = _context.Users.FirstOrDefault(u => u.Id == userId);

            Report.Id = userId;
            Report.Author = myUser.Name + " " + myUser.Family + " / " + myUser.InRole;
            Report.CreateDate = DateTime.Now;
            Report.View = 0;

            if (User.IsInRole(SD.Admin))
            {
                Report.IsSuccess = true;
            }
            else
            {
                Report.IsSuccess = false;
            }


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

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var myUser = _context.Users.FirstOrDefault(u => u.Id == userId);
            ViewData["FullName"] = myUser.Name + " " + myUser.Family;
        }

        public async Task<IActionResult> OnPostSaveDraftAsync()
        {
            if (!ModelState.IsValid)
            {
                initReportGroup();
                return Page();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var myUser = _context.Users.FirstOrDefault(u => u.Id == userId);

            Report.Id = userId;
            Report.Author = myUser.Name + " " + myUser.Family + " / " + myUser.InRole;
            Report.CreateDate = DateTime.Now;
            Report.View = 0;
            Report.IsSuccess = false; // پیش‌نویس‌ها منتشر نمی‌شوند
            Report.IsDraft = true; // این یک پیش‌نویس است

            _context.Reports.Add(Report);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}
