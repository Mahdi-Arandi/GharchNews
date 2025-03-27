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
using GharchNews.Roles;
using System.Security.Claims;
using GharchNews.Utilities;

namespace GharchNews.Pages.Admin.Report
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public IndexModel(GharchNews.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.Report> Report { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole(SD.Admin))
            {
                Report = await _context.Reports
               .Include(r => r.ReportGroup).Where(r => r.IsDraft == false || r.IsDraft==true && r.Id == userId).OrderByDescending(r => r.CreateDate).ToListAsync();
            }
            else
            {
                Report = await _context.Reports
               .Include(r => r.ReportGroup).Where(r => r.Id == userId).OrderByDescending(r => r.CreateDate).ToListAsync();
            }

            ViewData["ReturnUrl"] = "Index";
        }

        public IActionResult OnGetIsHotNews(int id)
        {
            var MyReport = _context.Reports.FirstOrDefault(r => r.ReportId == id);

            if (MyReport.IsHotNews == true)
            {
                MyReport.IsHotNews = false;
                _context.SaveChanges();
                return RedirectToAction("./index");
            }

            MyReport.IsHotNews = true;
            MyReport.HotNewsDate = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("./index");
        }

        public IActionResult OnGetIsSuccess(int id)
        {
            var MyReport = _context.Reports.FirstOrDefault(r => r.ReportId == id);

            if (MyReport.IsSuccess == true)
            {
                MyReport.IsSuccess = false;
                _context.SaveChanges();
                return RedirectToAction("./index");
            }

            MyReport.IsSuccess = true;
            _context.SaveChanges();
            return RedirectToAction("./index");
        }
    }
}
