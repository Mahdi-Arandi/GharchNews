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

namespace GharchNews.Pages.Admin.Report
{
    [Authorize(Roles = SD.Admin)]
    public class NotConfirmedModel : PageModel
    {
        private readonly GharchNews.Data.ApplicationDbContext _context;

        public NotConfirmedModel(GharchNews.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.Report> Report { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Report = await _context.Reports
           .Include(r => r.ReportGroup).Where(r => r.IsSuccess == false).OrderByDescending(r => r.CreateDate).ToListAsync();

            ViewData["ReturnUrl"] = "NotConfirmed";
        }

        public IActionResult OnGetIsSuccess(int id)
        {
            var MyReport = _context.Reports.FirstOrDefault(r => r.ReportId == id);

            if (MyReport.IsSuccess == false)
            {
                MyReport.IsSuccess = true;
                _context.SaveChanges();
            }

            return RedirectToAction("./NotConfirmed");
        }
    }
}
